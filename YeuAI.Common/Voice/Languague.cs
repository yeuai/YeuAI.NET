using System;

namespace YeuAI.Common.Voice
{
    /// <summary>
    /// List of supported language
    /// </summary>
    public enum Languague
    {
        English,
        Vietnamese
    }

    /// <summary>
    /// Google Language Parser
    /// </summary>
    public static class GoogleLanguageParser
    {
        /// <summary>
        /// Parse google supported language
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static string Parse(this Languague lang)
        {
            switch (lang)
            {
                case Languague.English:
                    return "en";
                case Languague.Vietnamese:
                    return "vi";
                default:
                    throw new NotImplementedException("Lang: " + lang);
            }
        }

        /// <summary>
        /// Parse google supported language
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static string Encode(this Languague lang)
        {
            return Parse(lang);
        }
    }
}
