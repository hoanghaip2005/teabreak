using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace App.ExtendMethods
{
    public static class AppExtends
    {
        public static void AppStatusCodePage(this IApplicationBuilder app)
        {
            // Handle status code errors (404, 500, etc.)
            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            
            // Handle unhandled exceptions
            app.UseExceptionHandler("/Error");
        }
    }
}