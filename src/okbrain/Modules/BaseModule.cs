using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Web;
using Nancy;
using okbrain.Models;

namespace okbrain.Modules
{
    public class BaseModule : NancyModule
    {

        public dynamic Model = new ExpandoObject();

        protected PageModel Page { get; set; }

        public BaseModule()
        {
            SetupModelDefaults();
        }

        public BaseModule(string modulepath)
            : base(modulepath)
        {
            SetupModelDefaults();
        }

        private void SetupModelDefaults()
        {
            Before += ctx =>
            {
                Page = new PageModel()
                {
                    IsAuthenticated = ctx.CurrentUser != null,
                    PreFixTitle = "okbrain",
                    CurrentUser = ctx.CurrentUser != null ? ctx.CurrentUser.UserName : "",
                    Errors = new List<ErrorModel>()
                };

                ViewBag.Page = Page;
                ViewBag.IsLive = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["Live"]);

                return null;
            };
        }
    }
}