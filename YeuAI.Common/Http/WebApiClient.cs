using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace YeuAI.Common.Http
{
    public class WebApiClient
    {
        private static WebApiClient __instance;
        private string username;
        private string password;
        private string apiRequestUrl;
        private string apiEndPointUrl;
        private MultipartFormDataContent formContent;
        private Dictionary<string, string> queryStrings;
        private Dictionary<string, string> requestHeaders;

        public WebApiClient()
        {
            this.formContent = new MultipartFormDataContent();
            this.queryStrings = new Dictionary<string, string>();
            this.requestHeaders = new Dictionary<string, string>();
        }

        /// <summary>
        /// Get instance
        /// </summary>
        private static WebApiClient Instance
        {
            get
            {
                if (__instance == null)
                {
                    __instance = new WebApiClient();
                }
                return __instance;
            }
        }

        public WebApiClient Reset()
        {
            this.SetCredentials(null, null);
            this.queryStrings.Clear();
            this.requestHeaders.Clear();
            this.formContent.Dispose();

            this.formContent = new MultipartFormDataContent();
            this.queryStrings = new Dictionary<string, string>();
            this.requestHeaders = new Dictionary<string, string>();
            return this;
        }

        public WebApiClient SetCredentials(string username, string password)
        {
            this.username = username;
            this.password = password;
            return this;
        }

        public WebApiClient SetHeader(string key, string value)
        {
            this.requestHeaders.Add(key, value);
            return this;
        }

        public WebApiClient SetApiUrl(string apiUrl, string endPointUri = "")
        {
            this.apiRequestUrl = apiUrl;
            this.apiEndPointUrl = endPointUri;
            return this;
        }

        public static WebApiClient NewRequest()
        {
            return new WebApiClient();
        }

        public static WebApiClient NewRequest(string apiUrl = "", string requestUrl = "", string userName = "", string password = "")
        {
            return NewRequest().Reset().SetApiUrl(apiUrl, requestUrl).SetCredentials(userName, password);
        }

        public WebApiClient AddFormData(string key, string value)
        {
            this.formContent.Add(new StringContent(value), key);
            return this;
        }

        public WebApiClient AddFormData(string key, Stream content)
        {
            this.formContent.Add(new StreamContent(content), key);
            return this;
        }

        public WebApiClient AddFormData(string key, Stream content, string fileName)
        {
            this.formContent.Add(new StreamContent(content), key, fileName);
            return this;
        }

        public WebApiClient AddQueryString(string key, object value)
        {
            this.queryStrings.Add(key, value?.ToString());
            return this;
        }

        /// <summary>
        /// Build current query
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public string BuildQuery(string endPoint = null)
        {
            using (var content = new FormUrlEncodedContent(this.queryStrings))
            {
                return string.Format("{0}?{1}", endPoint ?? this.apiEndPointUrl, content.ReadAsStringAsync().Result);
            }
        }

        /// <summary>
        /// Create new http client
        /// </summary>
        /// <returns></returns>
        public HttpClient BuildHttpClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(this.apiRequestUrl);

            foreach (var item in this.requestHeaders)
            {
                client.DefaultRequestHeaders.Add(item.Key, item.Value);
            }

            return client;
        }

        /// <summary>
        /// Execute GET method
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public async Task<TResult> GetAsync<TResult>()
        {
            using (var client = BuildHttpClient())
            {
                var result = await client.GetAsync(this.apiEndPointUrl);

                if (!result.IsSuccessStatusCode)
                {
                    throw new Exception($"Get request fail: StatusCode={result.StatusCode}, Message={result.ToString()}");
                }
                else
                {
                    return await result.Content.ReadAsAsync<TResult>();
                }
            }
        }

        /// <summary>
        /// Upload Async Form-Data
        /// Ref: https://stackoverflow.com/a/31535308/1896897
        /// Example:
        /// 
        /// var vUploadResult = await WebApiClient.NewRequest(vApiUrlYBDT, "api/upload")
        ///         .SetHeader("Authorization", "Bearer " + vGetYBDTLoginResult.id_token)
        ///         .AddFormData("file", xmlDataUpload, vGetPIDResult.PID + ".xml")
        ///         .UploadAsync<UploadResult>();
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<TResult> UploadAsync<TResult>()
        {
            using (var client = BuildHttpClient())
            {
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsync(this.apiEndPointUrl, formContent);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Post upload fail: StatusCode={response.StatusCode}, Message={response.ToString()}");
                }
                return await response.Content.ReadAsAsync<TResult>();
            }
        }

        public async Task<bool> UploadImage(byte[] imageBytes, string faceId, string userName, string userPhone = "", string dvRegister = "")
        {
            var formContent = new MultipartFormDataContent
            {
                //send form text values here
                { new StringContent(faceId), "faceId"},
                {new StringContent(userName), "userName" },
                {new StringContent(userPhone), "userPhone" },
                {new StringContent(dvRegister), "dvRegister" },
                // send Image Here
                {new StreamContent(new MemoryStream(imageBytes)), "userImage", "img-" + faceId + ".jpg"}
            };

            var myHttpClient = new HttpClient();
            var response = await myHttpClient.PostAsync(this.apiRequestUrl + "/checkin/user-new", formContent);
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Post json request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<T> PostJsonAsync<T>(object data)
        {
            using (var client = BuildHttpClient())
            {
                StringContent stringContent = null;
                if (data != null)
                {
                    var myContent = JsonConvert.SerializeObject(data);
                    stringContent = new StringContent(myContent, Encoding.UTF8, "application/json");
                }

                client.BaseAddress = new Uri(this.apiRequestUrl);
                var result = await client.PostAsync(this.apiEndPointUrl, stringContent);

                if (!result.IsSuccessStatusCode)
                {
                    throw new Exception($"Post json fail: StatusCode={result.StatusCode}, Message={result.ToString()}");
                }
                else
                {
                    return await result.Content.ReadAsAsync<T>();
                }
            }
        }

        /// <summary>
        /// Send download file request and save it to local folder
        /// </summary>
        /// <param name="saveFilePath"></param>
        public void DownloadFile(string saveFilePath)
        {
            using (var client = new WebClient())
            {
                client.BaseAddress = this.apiRequestUrl;
                client.DownloadFile(this.apiEndPointUrl, saveFilePath);
            }
        }

    }
}
