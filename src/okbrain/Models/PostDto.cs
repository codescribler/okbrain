using System;
using System.Collections.Generic;

namespace okbrain.Models
{
    public class PostDto : Dto
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public DateTime Date { get; set; }
        public string Body { get; set; }
        public List<string> Tags { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }

        public string Teaser{ get; set; }
        public string EmailTeaser { get; set; }
        

        public PostDto()
        {
            Status = "Draft";
            Teaser = "";
            EmailTeaser = "";
            Tags = new List<string>();
        }

        
    }
}