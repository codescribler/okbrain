using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Nancy;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Raven.Client;
using okbrain.Domain.Models;
using okbrain.Domain.Services;
using okbrain.Extensions;
using okbrain.Models;


namespace okbrain.Modules.Admin
{
    public class PostEditorModule : BaseModule
    {
        public PostEditorModule(PostService postService, IDocumentSession session) : base("/admin")
        {
            Get["/posts/new"] = parameters =>
                                    {
                                        var postDto = new PostDto();
                                        postDto.Body = "";
                                        postDto.Title = "";
                                        postDto.Status = "Draft";
                                        postDto.Date = DateTime.UtcNow;
                                        postDto.Tags = new List<string>();
                                        var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                                        ViewBag.JavaScript = JsonConvert.SerializeObject(postDto, Formatting.None, settings);
                return Negotiate.WithView("posteditor")
                .WithModel(postDto)
                .OrPartial(postDto);
            };

            Post["/posts/new"] = parameters =>
            {
                var postDate = GetPostPubDate(Request.Form.PubDate);
                Guid agId = Guid.NewGuid();
                var titles = new List<string>
                                        {
                                            Request.Form.Title
                                        };
                var createPost = new CreatePost(agId, Request.Form.Body, Request.Form.Teaser, Request.Form.EmailTeaser, titles,
                                                Request.Form.Status, postDate,
                                                "Daniel Whittaker", "");
                postService.Handles(createPost);

                PostDto lastPost = postService.GetLastPost(agId);

                var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                ViewBag.JavaScript = JsonConvert.SerializeObject(lastPost, Formatting.None, settings);

                return Negotiate.WithView("posteditor")
                    .WithModel(lastPost)
                    .OrPartial(lastPost);
            };

            Get["/posts/edit/id/{Id}"] = paramters =>
            {
                PostDto postDto = session.Load<PostDto>("PostDtos/" + (Guid)paramters.Id);

                var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                ViewBag.JavaScript = JsonConvert.SerializeObject(postDto, Formatting.None, settings);

                return Negotiate.WithView("posteditor")
                .WithModel(postDto)
                .OrPartial(postDto);
            };

            Post["/posts/edit/id"] = paramters =>
            {
                PostDto postDto = session.Load<PostDto>("PostDtos/" + (Guid)Request.Form.Id);

                postDto.Body = Request.Form.Body;
                postDto.Title = Request.Form.Title;
                postDto.Status = Request.Form.Status;
                postDto.Teaser = Request.Form.Teaser;
                postDto.EmailTeaser = Request.Form.EmailTeaser;
                postDto.Date = GetPostPubDate(Request.Form.PubDate);
                
                session.Store(postDto);
                session.SaveChanges();

                var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                ViewBag.JavaScript = JsonConvert.SerializeObject(postDto, Formatting.None, settings);

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

            Post["/posts/delete"] = parameters =>
                                        {
                                            session.Delete(session.Load<PostDto>("postDtos/" + Request.Form.Id));
                                            session.SaveChanges();
                                            
                                            var posts = session.Query<PostDto>();

                                            return Negotiate.WithView("adminpostlist")
                                                .WithModel(posts)
                                                .OrPartial(posts);
                                        };

        }

        private static DateTime GetPostPubDate(string pubDate)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime postDate = DateTime.ParseExact(pubDate, "dd/MM/yyyy", provider);
            return postDate;
        }
    }
}