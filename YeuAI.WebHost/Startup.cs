using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Newtonsoft.Json.Serialization;
using Owin;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using YeuAI.Common;
using YeuAI.WebApi.Http.Providers;

[assembly: OwinStartup(typeof(YeuAI.WebHost.Startup))]
namespace YeuAI.WebHost
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
            var viewRootPath = AppConfig.Get("Views", "Views");
            var viewRootData = AppConfig.Get("ViewNotFound", "Please contact Web Admin!");
            var viewRootIndex = Path.Combine(viewRootPath, "index.html");

            if (!Directory.Exists(viewRootPath))
            {
                Directory.CreateDirectory(viewRootPath);
            }

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
