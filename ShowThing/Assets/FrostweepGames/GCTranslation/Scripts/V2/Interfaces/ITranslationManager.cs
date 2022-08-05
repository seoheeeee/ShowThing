using System;

namespace FrostweepGames.Plugins.GoogleCloud.Translation.V2
{
    public interface ITranslationManager
    {
        event Action<TranslationResponse, long> TranslateSuccessEvent;
        event Action<DetectLanguageResponse, long> DetectLanguageSuccessEvent;
        event Action<LanguagesResponse, long> GetLanguagesSuccessEvent;

        event Action<string, long> TranslateFailedEvent;
        event Action<string, long> DetectLanguageFailedEvent;
        event Action<string, long> GetLanguagesFailedEvent;

        event Action ContentOutOfLengthEvent;

        long Translate(TranslationRequest translationRequest);
        long DetectLanguage(DetectLanguageRequest detectLanguageRequest);
        long GetLanguages(LanguagesRequest languagesRequest);
    }
}