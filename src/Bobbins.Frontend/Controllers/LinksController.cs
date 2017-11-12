using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bobbins.Frontend.Data;
using Bobbins.Frontend.Models.Comments;
using Bobbins.Frontend.Models.Links;
using Bobbins.Frontend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bobbins.Frontend.Controllers
{
    [Route("links")]
    [Authorize]
    public class LinksController : Controller
    {
        private readonly ILinkService _links;
        private readonly ICommentService _comments;

        public LinksController(ILinkService links, ICommentService comments)
        {
            _links = links;
            _comments = comments;
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

            var comments = (await _comments.Get(link.Id, ct).ConfigureAwait(false))
                           ?? new List<Comment>(0);

            var viewModel = new LinkPageViewModel
            {
                Link = link,
                Comments = comments
            };

            return View(viewModel);
        }
    }
}