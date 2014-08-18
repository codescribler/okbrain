using System.Collections.Generic;
using Nancy;
using Nancy.ModelBinding;

namespace okbrain.Modules
{
    public class IndexModule : BaseModule
    {
        public IndexModule()
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
                                    if (SimpleStore.Subscribers.Contains(email.Trim()))
                                    {
                                        cr.Message = "Already subscribed";
                                    }
                                    else
                                    {
                                        SimpleStore.Subscribers.Add(email.Trim());
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