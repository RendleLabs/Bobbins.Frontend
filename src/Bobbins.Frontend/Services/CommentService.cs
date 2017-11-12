using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Bobbins.Frontend.Models.Comments;
using Bobbins.Frontend.Options;
using Microsoft.Extensions.Options;

namespace Bobbins.Frontend.Services
{
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
            var response = await _http.GetAsync($"/comments/{linkId}/{id}", ct).ConfigureAwait(false);
            return await response.Deserialize<Comment>();
        }

        public async Task<List<Comment>> Get(int linkId, CancellationToken ct = default)
        {
            var response = await _http.GetAsync($"/links/{linkId}", ct).ConfigureAwait(false);
            return await response.Deserialize<List<Comment>>();
        }
    }
}