using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Bobbins.Frontend.Data;
using Bobbins.Frontend.Models.Comments;
using Bobbins.Frontend.Models.Links;
using Bobbins.Frontend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bobbins.Frontend.Controllers
{
    [Route("links")]
    [Authorize]
    public class LinksController : Controller
    {
        private readonly ILinkService _links;
        private readonly ICommentService _comments;
        private readonly ILogger<LinksController> _logger;

        public LinksController(ILinkService links, ICommentService comments, ILogger<LinksController> logger)
        {
            _links = links;
            _comments = comments;
            _logger = logger;
        }

        [HttpGet("create")]
        public IActionResult CreateForm()
        {
            return View(new NewLinkViewModel());
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] NewLinkViewModel viewModel, CancellationToken ct)
        {
            var link = new Link
            {
                Url = viewModel.Url,
                Title = viewModel.Title,
                User = User.FindFirst(BobbinsClaimTypes.ScreenName).Value,
                CreatedAt = DateTimeOffset.UtcNow,
            };
            link = await _links.Create(link, ct).ConfigureAwait(false);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("{id}"), AllowAnonymous]
        public async Task<IActionResult> View(int id, CancellationToken ct)
        {
            var link = await _links.Get(id, ct).ConfigureAwait(false);
            if (link == null) return NotFound();

            var comments = (await _comments.GetForLink(link.Id, ct).ConfigureAwait(false))
                           ?? new List<Comment>(0);

            var viewModel = new LinkPageViewModel
            {
                Link = link,
                Comments = comments,
                NewComment = new Comment { LinkId = id }
            };

            return View(viewModel);
        }

        [HttpGet("{id}/upvote"), HttpPut("{id}/upvote")]
        public async Task<IActionResult> UpVote(int id, [FromQuery]bool scripted, CancellationToken ct)
        {
            var link = await _links.Get(id, ct).ConfigureAwait(false);
            if (link == null) return scripted ? (IActionResult) NotFound() : RedirectToAction("Index", "Home");

            try
            {
                await _links.UpVote(id, User, ct).ConfigureAwait(false);
                return scripted ? (IActionResult) Accepted() : RedirectToAction("View", new {id});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in UpVote: {ex.Message}");
                return scripted ? (IActionResult) StatusCode(304) : RedirectToAction("View", new {id});
            }
        }

        [HttpGet("{id}/downvote"), HttpPut("{id}/downvote")]
        public async Task<IActionResult> DownVote(int id, [FromQuery]bool scripted, CancellationToken ct)
        {
            var link = await _links.Get(id, ct).ConfigureAwait(false);
            if (link == null) return scripted ? (IActionResult) NotFound() : RedirectToAction("Index", "Home");

            try
            {
                await _links.DownVote(id, User, ct).ConfigureAwait(false);
                return scripted ? (IActionResult) Accepted() : RedirectToAction("View", new {id});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in UpVote: {ex.Message}");
                return scripted ? (IActionResult) StatusCode(304) : RedirectToAction("View", new {id});
            }
        }
    }
}