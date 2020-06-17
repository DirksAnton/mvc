using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using WB_MVC.Middleware;

namespace WB_MVC.Extensions
{
    public static class appExtensions
    {
        public static IApplicationBuilder UseFileLogging(this IApplicationBuilder app)
                                                            => app.UseMiddleware<LogMiddleware>();
    }
}

