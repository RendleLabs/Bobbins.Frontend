using System.ComponentModel.DataAnnotations;

namespace Bobbins.Frontend.Models.Links
{
    public class NewLinkViewModel
    {
        [MaxLength(256)]
        public string Title { get; set; }
        [MaxLength(256), Url]
        public string Url { get; set; }
    }
}