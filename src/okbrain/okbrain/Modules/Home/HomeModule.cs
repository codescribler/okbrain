using Nancy;

namespace okbrain.Modules.Home
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = parameters =>
            {
                return View["index.cshtml"];
            };
        }
    }
}