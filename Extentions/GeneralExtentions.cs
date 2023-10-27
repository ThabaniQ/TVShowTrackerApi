using System.Linq;
using Microsoft.AspNetCore.Http;

namespace TVShowTracker.Extentions
{
    public static class GeneralExtentions
    {
        public static string GetUserId(this HttpContext httpContent ) 
        {
            if (httpContent.User == null)
            {
                return string.Empty;
            }
            return httpContent.User.Claims.Single(x => x.Type == "id").Value;
        }
    }
}
