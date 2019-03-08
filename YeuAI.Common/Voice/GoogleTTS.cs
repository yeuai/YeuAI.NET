using System;
using System.IO;
using System.Net;

namespace YeuAI.Common.Voice
{
    public class GoogleTTS
    {
        #region Constant
        private const string API_URL_GOOGLE_TTS = "http://translate.google.com/translate_tts";
        private const string MOZILLA_USER_AGENT = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)";
        #endregion

        /// <summary>
        /// Call the api and return buffer
        /// </summary>
        /// <param name="text"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static byte[] Request(string text, Languague lang)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("text");
            }

            using (var client = new WebClient())
            {
                client.Headers.Add("user-agent", MOZILLA_USER_AGENT);
                client.UseDefaultCredentials = true;
                client.QueryString.Add("ie", "UTF-8");
                client.QueryString.Add("client", "tw-ob");
                client.QueryString.Add("tl", lang.Encode());
                client.QueryString.Add("q", text);

                var response = client.DownloadData(API_URL_GOOGLE_TTS);
                return response;
            }
        }

        /// <summary>
        /// Call the api and generate temp audio file
        /// </summary>
        /// <param name="text"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static string DownloadFile(string text, Languague lang)
        {
            var result = Request(text, lang);
            var tmpFile = Path.GetTempFileName();
            tmpFile = Path.Combine(Path.GetTempPath(), $"yeuai_{Path.GetFileNameWithoutExtension(tmpFile)}.mp3");

            using (var file = File.OpenWrite(tmpFile))
            using (var stream = new MemoryStream(result))
            {
                stream.CopyTo(file);
                file.Flush();
                file.Close();
            }
            return tmpFile;
        }
    }
}
