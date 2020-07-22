using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_Forum.Models
{
    public class Reply
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string ReplyBody { get; set; }
        public int TopicId { get; set; }
    }
}
