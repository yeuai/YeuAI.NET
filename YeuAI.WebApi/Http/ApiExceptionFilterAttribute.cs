using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web.Http.Filters;

namespace YeuAI.WebApi.Http
{

    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var ex = context.Exception;
            var msg = new ApiExceptionMessage(ex);

            // log error
            //ex.LogDetail();

            if (ex is NotImplementedException)
            {
                msg.ErrorCode = ApiErrorCode.ERROR_NOT_IMPLEMENT;
                context.Response = context.Request.CreateResponse(HttpStatusCode.NotImplemented, msg);
            }
            else
            {
                context.Response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, msg);
            }
        }
    }

    public enum ApiErrorCode
    {
        OK = 0,
        SUCCESS = 0,
        ERROR_SERVER = 1,
        ERROR_NOT_IMPLEMENT = 2,
        ERROR_SQL = 3,
        ERROR_USER_INPUT = 4,
        ERROR_INVALID_REQUEST = 5
    }


    public class ApiExceptionMessage
    {
        /// <summary>
        /// Private fields
        /// </summary>
        private Exception exception;
        private ApiErrorCode code;
        private object data;

        /// <summary>
        /// Ham khoi tao voi thong so loi~ co so?
        /// </summary>
        /// <param name="ex"></param>
        public ApiExceptionMessage(Exception ex)
        {
            this.exception = ex;
            this.code = ApiErrorCode.ERROR_SERVER;
        }

        /// <summary>
        /// Ham khoi tao voi thong so loi~ co so?
        /// </summary>
        /// <param name="ex"></param>
        public ApiExceptionMessage(Exception ex, ApiErrorCode code)
            : this(ex)
        {
            this.code = code;
        }

        /// <summary>
        /// Ham khoi tao voi cac thong so loi~
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <param name="code">Error code</param>
        /// <param name="data">Typed object data</param>
        public ApiExceptionMessage(Exception ex, ApiErrorCode code, object data)
            : this(ex, code)
        {
            this.data = data;
        }

        /// <summary>
        /// Base exception
        /// </summary>
        [IgnoreDataMember]
        public Exception Exception => this.exception;

        /// <summary>
        /// Get type of Error
        /// </summary>
        public string ErrorType => this.exception.GetType().FullName;

        /// <summary>
        /// Exception error message
        /// </summary>
        public string Message => this.exception.Message;

        /// <summary>
        /// Exception error message by join inner exceptions
        /// </summary>
        public string ExceptionMessage
        {
            get
            {
                var inner = string.Join("->", this.InnerExceptions);
                return string.IsNullOrEmpty(inner) ? Message : inner;
            }
        }

        /// <summary>
        /// Error code
        /// </summary>
        public ApiErrorCode ErrorCode
        {
            get => this.code;
            set => this.code = value;
        }

        /// <summary>
        /// Exception stack trace
        /// </summary>
        public string ErrorTrace => this.exception.StackTrace;

        /// <summary>
        /// Get inner exceptions info
        /// </summary>
        public string[] InnerExceptions
        {
            get
            {
                var limit = 0;
                var result = new List<string>();
                var ex = this.exception.InnerException;
                while (ex != null && limit++ < 10)
                {
                    result.Add(ex.Message);
                    ex = ex.InnerException;
                }
                return result.ToArray();
            }
        }

        /// <summary>
        /// Data thong tin exception
        /// </summary>
        public object Data
        {
            get; set;
        }
    }

}
