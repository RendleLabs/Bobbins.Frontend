using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Bobbins.Frontend.Models.Comments;

namespace Bobbins.Frontend.Services
{
    public interface ICommentService
    {
        Task<Comment> Create(Comment comment, CancellationToken ct = default);
        Task<Comment> Get(int linkId, int id, CancellationToken ct = default);
        Task<List<Comment>> Get(int linkId, CancellationToken ct = default);
        Task UpVote(int id, ClaimsPrincipal user, CancellationToken ct = default);
        Task DownVote(int id, ClaimsPrincipal user, CancellationToken ct = default);
    }
}