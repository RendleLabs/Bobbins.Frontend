using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bobbins.Frontend.Models.Links;

namespace Bobbins.Frontend.Services
{
    public interface ILinkService
    {
        Task<Link> Create(Link link, CancellationToken ct = default);
        Task<List<Link>> Get(CancellationToken ct = default);
        Task<Link> Get(int id, CancellationToken ct = default);
    }
}