using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bobbins.Frontend.Models;
using Bobbins.Frontend.Models.Home;
using Bobbins.Frontend.Models.Links;
using Bobbins.Frontend.Services;

namespace Bobbins.Frontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILinkService _links;

        public HomeController(ILinkService links)
        {
            _links = links;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            List<Link> links;
            try
            {
                links = await _links.Get(ct).ConfigureAwait(false);
            }
            catch
            {
                links = new List<Link>();
            }
            var viewModel = new HomeViewModel
            {
                Links = links
            };
            return View(viewModel);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
