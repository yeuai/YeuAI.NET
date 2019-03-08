using Microsoft.Owin.StaticFiles.ContentTypes;

namespace YeuAI.Common.Http.Providers
{
    public class CustomContentTypeProvider : FileExtensionContentTypeProvider
    {
        public CustomContentTypeProvider()
        {
            // serve additional json files
            Mappings.Add(".json", "application/json");
        }
    }
}
