using Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeuAI.Common.Http.Providers
{
    public class NotFoundMiddleware : OwinMiddleware
    {
        /// <summary>
        /// Hàm khởi tạo middleware handle Request Not Found.
        /// Trả về view mặc định
        /// </summary>
        /// <param name="next"></param>
        /// <param name="app">App builder</param>
        /// <param name="viewNotFound"></param>
        public NotFoundMiddleware
            (
                OwinMiddleware next,
                IAppBuilder app,
                string viewNotFound
            )
            : base(next)
        {
            this.app = app;
            this.viewNotFound = viewNotFound;
        }

        // app views
        private readonly IAppBuilder app;
        private readonly string viewNotFound;

        /// <summary>
        /// Application builder
        /// </summary>
        public IAppBuilder App => this.app;

        /// <summary>
        /// View data response if api or static files not found
        /// </summary>
        public string ViewNotFound => viewNotFound;

        public override async Task Invoke(IOwinContext context)
        {
            await Next.Invoke(context);

            if (context.Response.StatusCode == 404)
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                context.Response.Write(this.viewNotFound);
            }
        }
    }
}
