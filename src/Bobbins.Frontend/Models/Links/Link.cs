using System;

namespace Bobbins.Frontend.Models.Links
{
    public class Link
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public int UpVoteCount { get; set; }

        public int DownVoteCount { get; set; }

        public int CommentCount { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string User { get; set; }
    }
}