using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using okbrain.Models;

namespace okbrain.Services
{
    public interface IAuthenticationService
    {
        Member GetLogin(string usernameoremail, string password);
        Member GetSocialLogin(string usernameoremail);
    }

    public class AuthenticationService : IAuthenticationService
    {
        dynamic DB;

        //public AuthenticationService(IDBFactory DBFactory)
        //{
        //    this.DB = DBFactory.DB();
        //}

        public Member GetLogin(string usernameoremail, string password)
        {
            //Nerd n = DB.Nerds.FindByUserNameAndPassword(usernameoremail, password);
            //n = n ?? DB.Nerds.FindByEmailAndPassword(usernameoremail, password);
            //return n;

            if (usernameoremail.ToLower() == "daniel" && password == "qwerty")
                return DemoMember.GetMember();

            return null;

        }

        public Member GetSocialLogin(string usernameoremail)
        {
            if (usernameoremail.ToLower() == "daniel@dreamfree.co.uk" || usernameoremail.ToLower() == "@codescribler")
                return DemoMember.GetMember();

            return null;
        }
    }

    public static class DemoMember
    {
        public static Member GetMember()
        {
            return new Member
            {
                Id = 1,
                Email = "daniel@dreamfree.co.uk",
                Claims = new[]
                                            {
                                                "Admin"
                                            },
                Guid = Guid.NewGuid().ToString(),
                Name = "Daniel",
                UserName = "DanielWhittaker"
            };
        }
    }

}