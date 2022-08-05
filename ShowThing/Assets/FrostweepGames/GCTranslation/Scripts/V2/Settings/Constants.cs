namespace FrostweepGames.Plugins.GoogleCloud.Translation.V2
{
    public class Constants
    {
        public const string POST_TRANSLATE_REQUEST_URL = "https://translation.googleapis.com/language/translate/v2";
        public const string POST_DETECT_REQUEST_URL = "https://translation.googleapis.com/language/translate/v2/detect";
        public const string GET_LANGUAGES_REQUEST_URL = "https://translation.googleapis.com/language/translate/v2/languages";
       
        public const string API_KEY_PARAM = "?key=";

        public const int MAX_LENGTH_OF_CONTENT = 362;
    }
}