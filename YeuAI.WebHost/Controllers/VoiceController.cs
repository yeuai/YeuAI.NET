using System.IO;
using System.Web.Http;
using YeuAI.Common.Crypto;
using YeuAI.Common.Http;
using YeuAI.Common.Http.Result;
using YeuAI.Common.Voice;
using YeuAI.WebHost;

namespace YeuAiNet.Voice.Controllers
{
    [RoutePrefix("voice")]
    public class VoiceController : ApiBaseController
    {
        [HttpGet]
        [Route("tts")]
        public IHttpActionResult GetVoice(string text, bool cache = true)
        {
            if (string.IsNullOrEmpty(text))
            {
                return BadRequest("text");
            }
            else if (text.Length > 200)
            {
                return BadRequest("text length > 200 characters");
            }
            if (cache)
            {
                var md5 = text.GetMD5Hash();
                var prefixHash = md5.Substring(0, WebServer.VoiceCacheHashPrefixLength);
                var folderName = Path.Combine("Cache", prefixHash);
                var cacheFileName = Path.Combine(folderName, md5 + ".mp3");
                if (File.Exists(cacheFileName))
                {
                    logger.Info("Get voice from cache: {0} -> {1}", text, cacheFileName);
                    return new FileResult(cacheFileName, "audio/mpeg");
                }

                var mp3 = GoogleTTS.Request(text, Languague.Vietnamese);

                Directory.CreateDirectory(folderName);
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
