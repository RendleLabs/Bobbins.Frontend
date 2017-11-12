using JetBrains.Annotations;

namespace Bobbins.Frontend.Models.Comments
{
    [PublicAPI]
    public class CommentVote
    {
        public int CommentId { get; set; }
        
        public int Value { get; set; }
        
        public string User { get; set; }
    }
}