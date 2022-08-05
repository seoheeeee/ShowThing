using FrostweepGames.Plugins.GoogleCloud.Translation.V2;
using System;
using UnityEngine;

namespace FrostweepGames.Plugins.GoogleCloud.Translation
{
    public class LocalizedText : MonoBehaviour
    {
        private UnityEngine.UI.Text _uiText;

        private TMPro.TextMeshProUGUI _tmproUGUIText;

        private TMPro.TextMeshPro _tmrpoText;

        public LocalizationItem localization;

        public bool localizeAtStart = true;

		private void Awake()
		{
            _uiText = GetComponent<UnityEngine.UI.Text>();
            _tmproUGUIText = GetComponent<TMPro.TextMeshProUGUI>();
            _tmrpoText = GetComponent<TMPro.TextMeshPro>();
        }

		private void Start()
		{
            if (localizeAtStart)
            {
                // COULD BE CHANGED TO NATIVE APP SYSTEM FOR LOCALIZATION
                localization.GetLocalization(Enumerators.TextLanguage.EN, Enumerators.TextLanguage.EN, (result) =>
                {
                    if (_uiText != null)
                        _uiText.text = result;

                    if (_tmproUGUIText != null)
                        _tmproUGUIText.text = result;

                    if (_tmrpoText != null)
                        _tmrpoText.text = result;
                });
            }
        }

		public class LocalizationItem
		{
            private long _localizationRequestId;
            private Action<string> _localizationCallback;

            public string originalValue;

            public void GetLocalization(Enumerators.TextLanguage sourceLanguage, Enumerators.TextLanguage targetLanguage, Action<string> localizationCallback)
            {
                _localizationCallback = localizationCallback;

                GCTranslationV2.Instance.TranslateSuccessEvent += TranslateSuccessEventHandler;

                _localizationRequestId = GCTranslationV2.Instance.Translate(new TranslationRequest()
                {
                    q = originalValue,
                    source = sourceLanguage.ToString(),
                    target = targetLanguage.ToString(),
                    format = Enumerators.TextFormatType.text.ToString(),
                    model = Enumerators.ModelType.@base.ToString()
                });
            }

			private void TranslateSuccessEventHandler(TranslationResponse response, long requestId)
			{
                if (requestId == _localizationRequestId && 
                    response != null && 
                    response.data  != null && 
                    response.data.translations != null && 
                    response.data.translations.Length > 0)
                {
                    _localizationCallback?.Invoke(response.data.translations[0].translatedText);
                }

                GCTranslationV2.Instance.TranslateSuccessEvent -= TranslateSuccessEventHandler;
            }
		}
    }
}