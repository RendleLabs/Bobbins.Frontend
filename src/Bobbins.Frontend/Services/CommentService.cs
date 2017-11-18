using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Bobbins.Frontend.Data;
using Bobbins.Frontend.Models.Comments;
using Bobbins.Frontend.Models.Links;
using Bobbins.Frontend.Options;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Bobbins.Frontend.Services
{
    [PublicAPI]
    public class CommentService : ICommentService
    {
        private readonly HttpClient _http;

        public CommentService(IOptions<ServiceOptions> options)
        {
            _http = new HttpClient
            {
                BaseAddress = new Uri(options.Value.Comments.BaseUrl)
            };
        }

        public async Task<Comment> Create(Comment comment, CancellationToken ct = default)
        {
            var response = await _http.PostJsonAsync("/comments", comment, ct).ConfigureAwait(false);
            return await response.Deserialize<Comment>();
        }

        public async Task<Comment> Get(int linkId, int id, CancellationToken ct = default)
        {
            var response = await _http.GetAsync($"/comments/{id}", ct).ConfigureAwait(false);
            return await response.Deserialize<Comment>();
        }

        public async Task<List<Comment>> GetForLink(int linkId, CancellationToken ct = default)
        {
            var response = await _http.GetAsync($"/comments/for-link/{linkId}", ct).ConfigureAwait(false);
            return await response.Deserialize<List<Comment>>();
        }
        
        public Task UpVote(int id, ClaimsPrincipal user, CancellationToken ct = default)
        {
            return Vote(id, user, 1, ct);
        }

        public Task DownVote(int id, ClaimsPrincipal user, CancellationToken ct = default)
        {
            return Vote(id, user, -1, ct);
        }

        private Task Vote(int id, ClaimsPrincipal user, int value, CancellationToken ct)
        {
            var vote = new CommentVote
            {
                CommentId = id,
                User = user.FindFirst(BobbinsClaimTypes.ScreenName)?.Value,
                Value = value
            };
            return _http.PutJsonAsync("/votes", vote, ct);
        }
    }
}