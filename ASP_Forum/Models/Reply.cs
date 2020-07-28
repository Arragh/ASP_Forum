using System;

namespace ASP_Forum.Models
{
    public class Reply
    {
        public Guid Id { get; private set; }
        public string UserName { get; set; }
        public string ReplyBody { get; set; }
        public Guid TopicId { get; set; }
    }
}
