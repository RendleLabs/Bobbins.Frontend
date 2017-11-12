using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Bobbins.Frontend.Models.Links;
using Bobbins.Frontend.Options;
using Microsoft.Extensions.Options;

namespace Bobbins.Frontend.Services
{
    public class LinkService : ILinkService
    {
        private readonly HttpClient _http;

        public LinkService(IOptions<ServiceOptions> options)
        {
            _http = new HttpClient
            {
                BaseAddress = new Uri(options.Value.Links.BaseUrl)
            };
        }

        public async Task<Link> Create(Link link, CancellationToken ct = default)
        {
            var response = await _http.PostJsonAsync("/links", link, ct).ConfigureAwait(false);
            return await response.Deserialize<Link>();
        }

        public async Task<List<Link>> Get(CancellationToken ct = default)
        {
            var response = await _http.GetAsync("/links", ct).ConfigureAwait(false);
            return await response.Deserialize<List<Link>>();
        }

        public async Task<Link> Get(int id, CancellationToken ct = default)
        {
            var response = await _http.GetAsync($"/links/{id}", ct).ConfigureAwait(false);
            return await response.Deserialize<Link>();
        }
    }
}