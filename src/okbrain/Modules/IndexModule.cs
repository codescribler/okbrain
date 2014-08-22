using System;
using System.Collections.Generic;
using Nancy;
using Nancy.ModelBinding;
using Raven.Client;
using Raven.Client.Linq;
using okbrain.Models;

namespace okbrain.Modules
{
    public class IndexModule : BaseModule
    {
        public IndexModule(IDocumentSession documentSession)
        {
            Get["/"] = parameters =>
                           {
                               return View["index"];
                           };

            Post["/"] = parameters =>
                            {
                                string email = Request.Form.email;
                                string real = Request.Form.real;

                                var cr = new CommonResult {Success = true, Data = null, Message = "Subscribed"};

                                if (!string.IsNullOrEmpty(real))
                                {
                                    cr.Success = false;
                                    cr.Data = null;
                                    cr.Message = "Bots - go away!";
                                }
                                else
                                {
                                    email = email.Trim().ToLower();

                                    RavenQueryStatistics stats;
                                    documentSession.Query<Subscription>()
                                                   .Where(s => s.Email == email)
                                                   .Statistics(out stats);

                                    if (stats.TotalResults > 0)
                                    {
                                        cr.Message = "Already subscribed";
                                    }
                                    else
                                    {
                                        var sub = new Subscription
                                                      {
                                                          Email = email,
                                                          Source = "HomePage",
                                                          SubscribedOn = DateTime.UtcNow
                                                      };
                                        documentSession.Store(sub);
                                        documentSession.SaveChanges();
                                    }
                                }

                                return Negotiate.WithView("index")
                                                .WithModel(cr)
                                                .OrPartial(cr);
                            };
        }
    }

    public class CommonResult
    {
        public bool Success { get; set; }
        public dynamic Data { get; set; }
        public string Message { get; set; }
    }

    public static class SimpleStore
    {
        public static List<string> Subscribers;

        static SimpleStore()
        {
            Subscribers = new List<string>();
        }
    }
   
}