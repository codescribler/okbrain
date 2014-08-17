namespace okbrain.Modules
{
    public class BlogModule : BaseModule
    {
        public BlogModule()
        {
            Get["/blog"] = parameters =>
            {
                return View["blog"];
            };
        }
    }
}