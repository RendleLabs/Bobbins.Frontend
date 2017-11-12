using JetBrains.Annotations;

namespace Bobbins.Frontend.Models.Links
{
    [PublicAPI]
    public class LinkVote
    {
        public int LinkId { get; set; }
        
        public int Value { get; set; }
        
        public string User { get; set; }
    }
}