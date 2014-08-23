using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace okbrain.Extensions
{
    public static class SlugExtension
    {
        public static string ToSlug(this string title)
        {
            return
                title.Replace("&", "-and-")
                     .Replace("'", "")
                     .Replace("(", "")
                     .Replace(")", "")
                     .Replace("<", "")
                     .Replace(">", "")
                     .Replace(" ", "-").Trim().ToLower();

        }
    }
}