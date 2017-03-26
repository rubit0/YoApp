using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin.Builder;
using Owin;
using Microsoft.AspNetCore.Builder;

namespace YoApp.Chat.Helpers
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    //https://github.com/aspnet/Entropy/tree/dev/samples/Owin.IAppBuilderBridge
    public static class OwinBuilderExtensions
    {
        /// <summary>
        /// Bridges IApplicationBuilder to IAppBuilder
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="configure">IAppBuilder</param>
        /// <returns>IApplicationBuilder</returns>
        public static IApplicationBuilder UseAppBuilder(this IApplicationBuilder app, Action<IAppBuilder> configure)
        {
            app.UseOwin(addToPipeline =>
            {
                addToPipeline(next =>
                {
                    var appBuilder = new AppBuilder();
                    appBuilder.Properties["builder.DefaultApp"] = next;

                    configure(appBuilder);

                    return appBuilder.Build<AppFunc>();
                });
            });

            return app;
        }
    }
}
