using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.Authentication.Forms;
using okbrain.Services;

namespace okbrain.Modules.Account
{
    public class AccountModule : BaseModule
    {
        public AccountModule(IAuthenticationService authenticationService)
        {
            //this.authenticationservice = authenticationService;

            Get["/login"] = parameters =>
                                {
                                    return View["login"];
                                };

            

            Post["/login"] = x =>
            {
                var member = authenticationService.GetLogin(Request.Form.Username, Request.Form.Password);

                if (member == null)
                    return Response.AsRedirect("/login?msg=Invalid%20username%20or%20password");

                DateTime? expiry = null;
                if (this.Request.Form.RememberMe.HasValue)
                {
                    expiry = DateTime.Now.AddDays(7);
                }

                Guid guid = Guid.Parse(member.Guid);

                return this.LoginAndRedirect(guid, expiry);
            };

            Get["/logout"] = x =>
            {
                return this.LogoutAndRedirect("/");
            };
        }
    }
}