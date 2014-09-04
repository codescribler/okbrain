using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Security;
using Raven.Client;
using okbrain.Services;

namespace okbrain.Models
{
    public class User : IUserMapper
    {
        private readonly IDocumentSession _session;
        public User(IDocumentSession documentStore)
        {
            _session = documentStore;
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            string id = identifier.ToString();
            //var member = _session.Query<Member>()
            //    .SingleOrDefault(x => x.Guid == id);
            Member member = DemoMember.GetMember();
            if (member.Guid == id) return member;

            if (member == null)
                return null;
            
          return member;

        }

    }

    public class Member : IUserIdentity
    {
        public Member()
        {
            Guid = System.Guid.NewGuid().ToString();
        }

        public int Id {get;set;}
        public string Name { get; set; }
        public string Guid { get; set; }
        public string Email {get;set;}
        public string PasswordSalt { get; set; }
        public string PasswordHashCode {get;set;}

        public IEnumerable<string> Claims
        {
            get;
            set;
        }

        public string UserName
        {
            get { return Name; }
            set { Name = value; }
        }
    }
   
}