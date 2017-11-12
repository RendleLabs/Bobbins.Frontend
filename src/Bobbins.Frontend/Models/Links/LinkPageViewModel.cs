using System.Collections.Generic;
using Bobbins.Frontend.Models.Comments;

namespace Bobbins.Frontend.Models.Links
{
    public class LinkPageViewModel
    {
        public Link Link { get; set; }
        public List<Comment> Comments { get; set; }
    }
}