using NLog;
using System.Collections;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YeuAI.Common.Logging;

namespace YeuAI.Common.Http
{
    [ApiExceptionFilter]
    public abstract class ApiBaseController : ApiController
    {
        #region Protected fields

        private ILogger _logger;

        protected ILogger logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = this.GetLogger();
                }
                return _logger;
            }
        }
        #endregion

        protected HttpResponseMessage HttpOk()
        {
            return HttpOk("Ok");
        }

        protected HttpResponseMessage HttpOk(object result)
        {
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, result);
        }

        protected HttpResponseMessage HttpBadRequest(object data)
        {
            return ControllerContext.Request.CreateResponse(HttpStatusCode.BadRequest, data);
        }

        protected HttpResponseMessage HttpNotFound(object data)
        {
            return ControllerContext.Request.CreateResponse(HttpStatusCode.NotFound, data);
        }

        protected HttpResponseMessage HttpForbidden(object data)
        {
            return ControllerContext.Request.CreateResponse(HttpStatusCode.Forbidden, data);
        }

        protected HttpResponseMessage HttpNotImplemented(object data)
        {
            return ControllerContext.Request.CreateResponse(HttpStatusCode.NotImplemented, data);
        }

        /// <summary>
        /// Tên gọi khác của HttpOk()
        /// </summary>
        /// <returns></returns>
        protected HttpResponseMessage ApiOk()
        {
            return HttpOk();
        }

        /// <summary>
        /// Tên gọi khác của HttpOk(object)
        /// Trả về mã lỗi: 200
        /// </summary>
        /// <returns></returns>
        protected HttpResponseMessage ApiOk(object result)
        {
            return HttpOk(result);
        }

        /// <summary>
        /// Tên gọi khác của HttpBadRequest(object)
        /// Trả về mã lỗi: 400
        /// </summary>
        /// <returns></returns>
        protected HttpResponseMessage ApiBadRequest(object data)
        {
            return HttpBadRequest(data);
        }

        /// <summary>
        /// Tiện ích trả về thông báo request không hợp lệ với nhiều điều kiện user inputs
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected HttpResponseMessage ApiBadRequest(params string[] data)
        {
            var result = string.Join(",", data);
            return HttpBadRequest(result);
        }

        /// <summary>
        /// Tên gọi khác của HttpNotFound(object)
        /// Trả về mã lỗi: 404
        /// </summary>
        /// <returns></returns>
        protected HttpResponseMessage ApiNotFound(object data)
        {
            return HttpNotFound(data);
        }

        /// <summary>
        /// Tiện ích trả về thông báo ko tìm thấy với nhiều điều kiện user inputs
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected HttpResponseMessage ApiNotFound(params string[] data)
        {
            var result = string.Join(",", data);
            return HttpNotFound(result);
        }


        /// <summary>
        /// Tên gọi khác của HttpForbidden(object)
        /// Trả về mã lỗi: 403
        /// </summary>
        /// <returns></returns>
        protected HttpResponseMessage ApiForbidden(object data)
        {
            return HttpForbidden(data);
        }

        /// <summary>
        /// Tên gọi khác của HttpNotImplemented(object)
        /// Trả về mã lỗi: 501
        /// </summary>
        /// <returns></returns>
        protected HttpResponseMessage ApiNotImplemented(object data = null)
        {
            return HttpNotImplemented(data);
        }

        /// <summary>
        /// Trả về danh sách dữ liệu dạng mảng, danh sách hoặc collection
        /// </summary>
        /// <param name="list"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        protected HttpResponseMessage ApiList(ICollection list, int count)
        {
            return HttpOk(new
            {
                Data = list,
                Count = count
            });
        }

    }
}
