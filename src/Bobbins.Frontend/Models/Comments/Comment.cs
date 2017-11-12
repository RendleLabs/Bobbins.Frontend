using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Bobbins.Frontend.Models.Comments
{
    [PublicAPI]
    public class Comment
    {
        public int Id { get; set; }
        public int LinkId { get; set; }
        public int? ReplyToId { get; set; }
        public int UpVoteCount { get; set; }
        public int DownVoteCount { get; set; }
        public int ReplyCount { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string User { get; set; }
        public string Text { get; set; }
        public List<Comment> Replies { get; set; }
    }
}