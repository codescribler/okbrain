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

                                var cr = new CommonResponse {Success = true, Data = null, Message = "Subscribed"};

                                if (!string.IsNullOrEmpty(real))
                                {
                                    cr.Success = false;
                                    cr.Data = null;
                                    cr.Message = "Bots - go away!";
                                }

                                return Negotiate.WithView("index")
                                                .WithModel(cr)
                                                .OrPartial(cr);
                            };
        }
    }

    public class CommonResponse
    {
        public bool Success { get; set; }
        public dynamic Data { get; set; }
        public string Message { get; set; }
    }

   
}