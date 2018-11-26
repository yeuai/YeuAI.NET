using Microsoft.Owin.StaticFiles.ContentTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YeuAiNet.Voice.Providers
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
