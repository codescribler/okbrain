using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace okbrain.Modules
{
    public class PostModule : BaseModule
    {
        public PostModule()
        {
            Get["/blog/posts/new"] = parameters =>
                                         {
                                             return View["newpost"];
                                         };
        }
    }
}