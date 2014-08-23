using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Raven.Client;
using okbrain.Domain.Models;
using okbrain.Domain.Services;

namespace okbrain.Modules.Admin
{
    public class PostEditorModule : BaseModule
    {
        public PostEditorModule(PostService postService) : base("/admin")
        {
            Get["/posts/new"] = parameters =>
            {
                return View["posteditor"];
            };

            Post["/posts/new"] = parameters =>
                                     {
                                         CultureInfo provider = CultureInfo.InvariantCulture;
                                         DateTime postDate = DateTime.ParseExact(Request.Form.PubDate.ToString(), "DD/MM/yyyy", provider);
                                         var titles = new List<string>
                                                                   {
                                                                       Request.Form.Title
                                                                   };
                                         var createPost = new CreatePost(Request.Form.Body, titles,
                                                                         Request.Form.Status, postDate,
                                                                         "Daniel Whittaker", "");
                                         postService.Handles(createPost);

                                         return View["posteditor"];
                                     };

        }
    }
}