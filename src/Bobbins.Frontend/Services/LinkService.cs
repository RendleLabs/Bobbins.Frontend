﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Bobbins.Frontend.Data;
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

        public Task UpVote(int id, ClaimsPrincipal user, CancellationToken ct = default)
        {
            return Vote(id, user, 1, ct);
        }

        public Task DownVote(int id, ClaimsPrincipal user, CancellationToken ct = default)
        {
            return Vote(id, user, -1, ct);
        }

        public Task CommentAdded(int id, CancellationToken ct = default)
        {
            return _http.PutAsync($"/links/{id}/comment-added", new StringContent(string.Empty), ct);
        }

        private Task Vote(int id, ClaimsPrincipal user, int value, CancellationToken ct)
        {
            var vote = new LinkVote
            {
                LinkId = id,
                User = user.FindFirst(BobbinsClaimTypes.ScreenName)?.Value,
                Value = value
            };
            return _http.PutJsonAsync("/votes", vote, ct);
        }
    }
}