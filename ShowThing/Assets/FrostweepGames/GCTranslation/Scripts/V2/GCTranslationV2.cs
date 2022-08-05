using UnityEngine;
using System;
using FrostweepGames.Plugins.Core;

namespace FrostweepGames.Plugins.GoogleCloud.Translation.V2
{
    public class GCTranslationV2 : MonoBehaviour
    {
        public event Action<TranslationResponse, long> TranslateSuccessEvent;
        public event Action<DetectLanguageResponse, long> DetectLanguageSuccessEvent;
        public event Action<LanguagesResponse, long> GetLanguagesSuccessEvent;

        public event Action<string, long> TranslateFailedEvent;
        public event Action<string, long> DetectLanguageFailedEvent;
        public event Action<string, long> GetLanguagesFailedEvent;

        public event Action ContentOutOfLengthEvent;

        private static GCTranslationV2 _Instance;
        public static GCTranslationV2 Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new GameObject("[Singleton]GCTranslationV2").AddComponent<GCTranslationV2>();
                }

                return _Instance;
            }
        }

        private ITranslationManager _translationManager;

        [Header("Prefab Object Settings")]
        public bool isDontDestroyOnLoad = false;
        public bool isFullDebugLogIfError = false;

        [Header("Prefab Fields")]
        [PasswordField]
        public string apiKey = string.Empty;

        private void Awake()
        {
            if (_Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            if (isDontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);

            _Instance = this;

            ServiceLocator.Register<ITranslationManager>(new TranslationManager());
            ServiceLocator.InitServices();

            _translationManager = ServiceLocator.Get<ITranslationManager>();

            _translationManager.TranslateSuccessEvent += TranslateSuccessEventHandler;
            _translationManager.DetectLanguageSuccessEvent += DetectLanguageSuccessEventHandler;
            _translationManager.GetLanguagesSuccessEvent += GetLanguagesSuccessEventHandler;

            _translationManager.TranslateFailedEvent += TranslateFailedEventHandler;
            _translationManager.DetectLanguageFailedEvent += DetectLanguageFailedEventHandler;
            _translationManager.GetLanguagesFailedEvent += GetLanguagesFailedEventHandler;

            _translationManager.ContentOutOfLengthEvent += ContentOutOfLengthEventHandler;
        }

        private void Update()
        {
            if (_Instance == this)
            {
                ServiceLocator.Instance.Update();
            }
        }

        private void OnDestroy()
        {
            if (_Instance == this)
            {
                _translationManager.TranslateSuccessEvent -= TranslateSuccessEventHandler;
                _translationManager.DetectLanguageSuccessEvent -= DetectLanguageSuccessEventHandler;
                _translationManager.GetLanguagesSuccessEvent -= GetLanguagesSuccessEventHandler;

                _translationManager.TranslateFailedEvent -= TranslateFailedEventHandler;
                _translationManager.DetectLanguageFailedEvent -= DetectLanguageFailedEventHandler;
                _translationManager.GetLanguagesFailedEvent -= GetLanguagesFailedEventHandler;

                _translationManager.ContentOutOfLengthEvent -= ContentOutOfLengthEventHandler;

                _Instance = null;
                ServiceLocator.Instance.Dispose();
            }
        }               

        public long Translate(TranslationRequest translationRequest)
        {
           return _translationManager.Translate(translationRequest);
        }

        public long DetectLanguage(DetectLanguageRequest detectLanguageRequest)
        {
            return _translationManager.DetectLanguage(detectLanguageRequest);
        }

        public long GetLanguages(LanguagesRequest languagesRequest)
        {
            return _translationManager.GetLanguages(languagesRequest);
        }

        private void TranslateSuccessEventHandler(TranslationResponse value, long requestId)
        {
            if (TranslateSuccessEvent != null)
                TranslateSuccessEvent(value, requestId);
        }

        private void DetectLanguageSuccessEventHandler(DetectLanguageResponse value, long requestId)
        {
            if (DetectLanguageSuccessEvent != null)
                DetectLanguageSuccessEvent(value, requestId);
        }

        private void GetLanguagesSuccessEventHandler(LanguagesResponse value, long requestId)
        {
            if (GetLanguagesSuccessEvent != null)
                GetLanguagesSuccessEvent(value, requestId);
        }

        private void TranslateFailedEventHandler(string value, long requestId)
        {
            if (TranslateFailedEvent != null)
                TranslateFailedEvent(value, requestId);
        }

        private void DetectLanguageFailedEventHandler(string value, long requestId)
        {
            if (DetectLanguageFailedEvent != null)
                DetectLanguageFailedEvent(value, requestId);
        }

        private void GetLanguagesFailedEventHandler(string value, long requestId)
        {
            if (GetLanguagesFailedEvent != null)
                GetLanguagesFailedEvent(value, requestId);
        }

        private void ContentOutOfLengthEventHandler()
        {
            if (ContentOutOfLengthEvent != null)
                ContentOutOfLengthEvent();
        }
    }
}