using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace okbrain.Models
{
    public interface IDto
    {
        Guid Id { get; set; }
    }

    public abstract class Dto : IDto
    {
        protected Dto()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
    }

    public class Subscription : Dto
    {
        public string Email { get; set; }
        public DateTime SubscribedOn{ get; set; }
        public string Source { get; set; }
    }

    public class PostDto : Dto
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public DateTime Date { get; set; }
        public string Body { get; set; }
        public List<string> Tags { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }

        public PostDto()
        {
            Status = "Draft";
        }

        
    }
}