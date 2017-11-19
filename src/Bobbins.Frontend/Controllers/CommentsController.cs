using System;
using System.Threading;
using System.Threading.Tasks;
using Bobbins.Frontend.Data;
using Bobbins.Frontend.Models.Comments;
using Bobbins.Frontend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bobbins.Frontend.Controllers
{
    [Authorize]
    [Route("comments")]
    public class CommentsController : Controller
    {
        private readonly ILogger<CommentsController> _logger;
        private readonly ILinkService _links;
        private readonly ICommentService _comments;

        public CommentsController(ILogger<CommentsController> logger, ICommentService comments, ILinkService links)
        {
            _logger = logger;
            _comments = comments;
            _links = links;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Comment comment, CancellationToken ct)
        {
            comment.User = User.FindFirst(BobbinsClaimTypes.ScreenName).Value;
            comment = await _comments.Create(comment, ct).ConfigureAwait(false);
            await _links.CommentAdded(comment.LinkId, ct).ConfigureAwait(false);
            return RedirectToAction("View", "Links", new {id = comment.LinkId});
        }

        [HttpGet("{linkId}/{id}")]
        public async Task<IActionResult> View(int linkId, int id, CancellationToken ct)
        {
            _logger.LogInformation($"Comments.View({linkId}, {id})");
            var comment = await _comments.Get(linkId, id, ct);
            if (comment == null) return NotFound();
            var viewModel = new CommentPageViewModel
            {
                Comment = comment,
                NewComment = new Comment
                {
                    LinkId = linkId,
                    ReplyToId = id
                }
            };
            return View(viewModel);
        }

        [HttpPut("{linkId}/{id}/upvote")]
        public async Task<IActionResult> UpVote(int linkId, int id, CancellationToken ct)
        {
            var comment = await _comments.Get(linkId, id, ct).ConfigureAwait(false);
            if (comment == null) return NotFound();

            try
            {
                await _comments.UpVote(id, User, ct).ConfigureAwait(false);
                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in UpVote: {ex.Message}");
                return StatusCode(304);
            }
        }

        [HttpPut("{linkId}/{id}/downvote")]
        public async Task<IActionResult> DownVote(int linkId, int id, CancellationToken ct)
        {
            var comment = await _comments.Get(linkId, id, ct).ConfigureAwait(false);
            if (comment == null) return NotFound();

            try
            {
                await _comments.DownVote(id, User, ct).ConfigureAwait(false);
                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in UpVote: {ex.Message}");
                return StatusCode(304);
            }
        }
    }
}