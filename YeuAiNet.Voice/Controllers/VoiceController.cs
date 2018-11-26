using NLog;
using System.IO;
using System.Web.Http;
using YeuAiNet.Common.Logging;
using YeuAiNet.Common.Crypto;
using YeuAiNet.Google.TTS;
using YeuAiNet.Voice.Responses;

namespace YeuAiNet.Voice.Controllers
{
    [RoutePrefix("voice")]
    public class VoiceController : ApiController
    {
        protected ILogger logger = AppLogger.GetLogger("voice-api");

        [HttpGet]
        [Route("tts")]
        public IHttpActionResult GetVoice(string text, bool cache = true)
        {
            if (string.IsNullOrEmpty(text))
            {
                return BadRequest("text");
            }
            if (cache)
            {
                var md5 = text.GetMD5Hash();
                var prefixHash = md5.Substring(0, 4);
                var cacheFileName = Path.Combine("Cache", prefixHash, md5 + ".mp3");
                if (File.Exists(cacheFileName))
                {
                    logger.Info("Get voice from cache: {0} -> {1}", text, cacheFileName);
                    return new FileResult(cacheFileName, "audio/mpeg");
                }

                var mp3 = GoogleTTS.Request(text, Languague.Vietnamese);

                Directory.CreateDirectory(prefixHash);
                using (var file = File.OpenWrite(cacheFileName))
                using (var stream = new MemoryStream(mp3))
                {
                    stream.CopyTo(file);
                    file.Flush();
                    file.Close();
                    File.WriteAllText(Path.Combine("Cache", prefixHash, md5 + ".txt"), text);
                }
                return new ByteArrayResult(mp3, "audio/mpeg");
            }
            else
            {
                logger.Info("Get voice without cache: {0}", text);
                var mp3 = GoogleTTS.Request(text, Languague.Vietnamese);
                return new ByteArrayResult(mp3, "audio/mpeg");
            }
        }
    }
}
