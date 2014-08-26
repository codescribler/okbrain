using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Raven.Client;
using Raven.Client.Linq;
using okbrain.Models;

namespace okbrain.Modules
{
    public class BlogModule : BaseModule
    {
        public BlogModule(IDocumentSession session)
        {
            Get["/blog"] = parameters =>
            {
                var posts = session.Query<PostDto>().Where(b => b.Status == "Live" && b.Date <= DateTime.UtcNow).OrderBy(b => b.Date);
                var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                ViewBag.JavaScript = JsonConvert.SerializeObject(posts, Formatting.None, settings);
                return Negotiate.WithView("blog")
                    .WithModel(posts)
                    .OrPartial(posts);
                
            };

            Get["/blog/{slug}"] = parameters =>
                                      {
                                          var slug = (string)parameters.Slug;
                                          var date = DateTime.UtcNow;
                                          PostDto post =
                                              session.Query<PostDto>()
                                                     .Where(
                                                         b =>
                                                         b.Slug == slug && b.Status == "Live" &&
                                                         b.Date <= date).ToList().FirstOrDefault();
                                          var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                                          ViewBag.JavaScript = JsonConvert.SerializeObject(post, Formatting.None, settings);
                                          return Negotiate.WithView("post")
                                              .WithModel(post)
                                              .OrPartial(post);
                                          
                                      };
        }
    }
}