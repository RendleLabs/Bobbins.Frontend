using System;

namespace Bobbins.Frontend.Models.Comments
{
    public class Comment
    {
        public int Id { get; set; }
        public int LinkId { get; set; }
        public int UpVoteCount { get; set; }
        public int DownVoteCount { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string User { get; set; }
        public string Text { get; set; }
    }
}