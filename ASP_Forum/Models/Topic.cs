using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_Forum.Models
{
    public class Topic
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public string UserName { get; set; }
        public int SectionId { get; set; }
    }
}
