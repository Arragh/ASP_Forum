using System;
using System.ComponentModel.DataAnnotations;

namespace ASP_Forum.Models
{
    public class Section
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
    }
}
