using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Nancy;
using Raven.Client;
using okbrain.Domain.Models;
using okbrain.Domain.Services;
using okbrain.Models;


namespace okbrain.Modules.Admin
{
    public class PostEditorModule : BaseModule
    {
        public PostEditorModule(PostService postService, IDocumentSession session) : base("/admin")
        {
            Get["/posts/new"] = parameters =>
            {
                return View["posteditor"];
            };

            Post["/posts/new"] = parameters =>
            {
                string pubDate = Request.Form.PubDate;


                CultureInfo provider = CultureInfo.InvariantCulture;
                DateTime postDate = DateTime.ParseExact(pubDate, "dd/MM/yyyy", provider);
                Guid agId = Guid.NewGuid();
                var titles = new List<string>
                                        {
                                            Request.Form.Title
                                        };
                var createPost = new CreatePost(agId, Request.Form.Body, titles,
                                                Request.Form.Status, postDate,
                                                "Daniel Whittaker", "");
                postService.Handles(createPost);

                PostDto lastPost = postService.GetLastPost(agId);

                return Negotiate.WithView("posteditor")
                    .WithModel(lastPost)
                    .OrPartial(lastPost);
            };

            Get["/posts/edit/id/{Id}"] = paramters =>
            {
                PostDto postDto = session.Load<PostDto>("PostDtos/" + (Guid)paramters.Id);

                return Negotiate.WithView("posteditor")
                .WithModel(postDto)
                .OrPartial(postDto);
            };

            Get["/posts"] = parameters =>
                                {
                                    var posts = session.Query<PostDto>();
                                    return Negotiate.WithView("adminpostlist")
                                        .WithModel(posts)
                                        .OrPartial(posts);
                                };

        }

    }
}