using Microsoft.Owin;
using Owin;
using System;
using System.Web.Http;
using Microsoft.Owin.Cors;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using System.IO;
using System.Net.Http.Formatting;
using System.Linq;
using Newtonsoft.Json.Serialization;
using YeuAiNet.Common;
using YeuAiNet.Voice.Providers;

[assembly: OwinStartup(typeof(YeuAiNet.Voice.Startup))]
namespace YeuAiNet.Voice
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Always on the first line (enable cors and allow all)
            // Ref: https://stackoverflow.com/a/33189638/1896897
            app.UseCors(CorsOptions.AllowAll);

            this.ConfigureWebApi(app);
            this.ConfigureWebView(app);

        }

        /// <summary>
        /// 03. Cấu hình web api quản trị hệ thống tài khoản
        /// </summary>
        /// <param name="config"></param>
        private void ConfigureWebApi(IAppBuilder app)
        {
            app.Map("/api", map =>
            {
                var config = new HttpConfiguration();
                config.MapHttpAttributeRoutes();

                // Require authentication for all controllers
                // If you don't want check authentication for api, just comment this below line.
                //config.Filters.Add(new AuthorizeAttribute());

                var jsonFormater = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
                jsonFormater.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                map.UseWebApi(config);
            });

           
        }

        /// <summary>
        /// Config view handlers
        /// </summary>
        /// <param name="app"></param>
        private void ConfigureWebView(IAppBuilder app)
        {
            // Default Views
            var viewRootPath = AppConfiguration.Get("Views", "Views");
            var viewRootData = AppConfiguration.Get("ViewNotFound", "Please contact Web Admin!");
            var viewRootIndex = Path.Combine(viewRootPath, "index.html");

            // Views Data
            if (File.Exists(viewRootIndex))
            {
                viewRootData = File.ReadAllText(viewRootIndex);
            }

            // Static File Server
            var options = new FileServerOptions
            {
                // Chỉ enable đối với đường dẫn quản trị và có quyền xem
                EnableDirectoryBrowsing = false,
                EnableDefaultFiles = true,
                DefaultFilesOptions = { DefaultFileNames = { "index.html" } },
                FileSystem = new PhysicalFileSystem(viewRootPath),
                StaticFileOptions = { ContentTypeProvider = new CustomContentTypeProvider() }
            };

            app
                .Use<NotFoundMiddleware>(app, viewRootData)
                .UseFileServer(options);
        }
    }
}
