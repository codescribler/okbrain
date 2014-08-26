using System;
using System.Collections.Generic;
using okbrain.Domain.Messages;
using okbrain.Models;
using okbrain.Services;

namespace okbrain.Domain.Models
{
    public class Post
    {
        private PostDto _post;

        public void CreatePost(CreatePost command, ITaxonomy taxonomy)
        {
            _post = new PostDto();
            _post.Id = command.AgId;
            _post.Slug = command.Slug;
            _post.Title = command.Titles[0];
            _post.Body = command.Body;
            _post.Date = command.PostDate;
            _post.Tags = taxonomy.GenerateTags(_post.Body);
            _post.Teaser = command.Teaser;
            _post.EmailTeaser = command.EmailTeaser;

        }

        public PostDto GetDto()
        {
            return _post;
        }
    }

    public class CreatePost : Command
    {
        public readonly string Body;
        public readonly string Teaser;
        public readonly string EmailTeaser;
        public readonly List<string> Titles;
        public readonly string Status;
        public readonly DateTime PostDate;
        public readonly string Author;
        public readonly string Slug;

        public CreatePost(Guid agId, string body, string teaser, string emailTeaser, List<string> titles, string status, DateTime postDate, string author, string slug)
        {
            AgId = agId;
            Body = body;
            EmailTeaser = emailTeaser;
            Teaser = teaser;
            Titles = titles;
            Status = status;
            PostDate = postDate;
            Author = author;
            Slug = slug;
        }
    }
}