using UnityEngine;
using System;
using Newtonsoft.Json;
using FrostweepGames.Plugins.Core;
using FrostweepGames.Plugins.Networking;
using System.Collections.Generic;

namespace FrostweepGames.Plugins.GoogleCloud.Translation.V2
{
    public class TranslationManager : IService, IDisposable, ITranslationManager
    {
        public event Action<TranslationResponse, long> TranslateSuccessEvent;
        public event Action<DetectLanguageResponse, long> DetectLanguageSuccessEvent;
        public event Action<LanguagesResponse, long> GetLanguagesSuccessEvent;

        public event Action<string, long> TranslateFailedEvent;
        public event Action<string, long> DetectLanguageFailedEvent;
        public event Action<string, long> GetLanguagesFailedEvent;

        public event Action ContentOutOfLengthEvent;

        private NetworkingService _networking;
        private GCTranslationV2 _gcTranslation;

        public void Init()
        {
            _gcTranslation = GCTranslationV2.Instance;

            _networking = new NetworkingService();
            _networking.NetworkResponseEvent += NetworkResponseEventHandler;
        }

        public void Update()
        {
            _networking.Update();
        }

        public void Dispose()
        {
            _networking.NetworkResponseEvent -= NetworkResponseEventHandler;
            _networking.Dispose();
        }

        public long Translate(TranslationRequest translationRequest)
        {
            if (!CheckOnContentLength(translationRequest.q))
                return -1;

           return SendRequestInternal(JsonConvert.SerializeObject(translationRequest), Enumerators.GoogleCloudRequestType.TRANSLATE);
        }

        public long DetectLanguage(DetectLanguageRequest detectLanguageRequest)
        {
            if (!CheckOnContentLength(detectLanguageRequest.q))
                return -1;

            return SendRequestInternal(JsonConvert.SerializeObject(detectLanguageRequest), Enumerators.GoogleCloudRequestType.DETECT_LANGUAGE);
        }

        public long GetLanguages(LanguagesRequest languagesRequest)
        {
            return SendRequestInternal(JsonConvert.SerializeObject(languagesRequest), Enumerators.GoogleCloudRequestType.GET_LANGUAGES);
        }

        private bool CheckOnContentLength(string content)
        {
            if (content.Length > Constants.MAX_LENGTH_OF_CONTENT)
            {
#if UNITY_EDITOR
                Debug.Log("Error: Text To Translate is too biggest. should be less then " + Constants.MAX_LENGTH_OF_CONTENT + " characters");
#endif
                if (ContentOutOfLengthEvent != null)
                    ContentOutOfLengthEvent();
                return false;
            }

            return true;
        }

        private long SendRequestInternal(string postData, Enumerators.GoogleCloudRequestType cloudRequestType)
        {
            string uri = string.Empty;
            NetworkEnumerators.RequestType requestType = NetworkEnumerators.RequestType.POST;

            switch (cloudRequestType)
            {
                case Enumerators.GoogleCloudRequestType.TRANSLATE:
                    uri += Constants.POST_TRANSLATE_REQUEST_URL;
                    break;
                case Enumerators.GoogleCloudRequestType.DETECT_LANGUAGE:
                    uri += Constants.POST_DETECT_REQUEST_URL;
                    break;
                case Enumerators.GoogleCloudRequestType.GET_LANGUAGES:
                    uri += Constants.GET_LANGUAGES_REQUEST_URL;
                 //   requestType = NetworkEnumerators.RequestType.GET; // on offical Google page it looks like GET request but it works correctly only with POST request
                    break;
                default: break;
            }

            uri += Constants.API_KEY_PARAM + _gcTranslation.apiKey;

            return _networking.SendRequest(uri, postData, requestType, GetHeaders(), new object[] { cloudRequestType });
        }

        private void NetworkResponseEventHandler(NetworkResponse Response)
        {
            Enumerators.GoogleCloudRequestType googleCloudRequestType = (Enumerators.GoogleCloudRequestType)Response.Parameters[0];

            if (_gcTranslation.isFullDebugLogIfError)
                Debug.Log(Response.Error + "\n" + Response.Response);

            if (!string.IsNullOrEmpty(Response.Error))
            {
                ThrowFailedEvent(Response.Error + "\n" + Response.Response, googleCloudRequestType, Response.RequestId);
            }
            else
            {
                if (Response.Response.Trim(' ').Contains("\"error\":{"))
                {
                    ThrowFailedEvent(Response.Error + "\n" + Response.Response, googleCloudRequestType, Response.RequestId);
                }
                else
                    ThrowSuccessEvent(Response.Response, googleCloudRequestType, Response.RequestId);
            }
        }

        private void ThrowFailedEvent(string Error, Enumerators.GoogleCloudRequestType type, long requestId)
        {
            switch (type)
            {
                case Enumerators.GoogleCloudRequestType.TRANSLATE:
                    {
                        if (TranslateFailedEvent != null)
                            TranslateFailedEvent(Error, requestId);
                    }
                    break;
                case Enumerators.GoogleCloudRequestType.DETECT_LANGUAGE:
                    {
                        if (DetectLanguageFailedEvent != null)
                            DetectLanguageFailedEvent(Error, requestId);
                    }
                    break;
                case Enumerators.GoogleCloudRequestType.GET_LANGUAGES:
                    {
                        if (GetLanguagesFailedEvent != null)
                            GetLanguagesFailedEvent(Error, requestId);
                    }
                    break;
                default: break;
            }
        }

        private void ThrowSuccessEvent(string data, Enumerators.GoogleCloudRequestType type, long requestId)
        {
            switch (type)
            {
                case Enumerators.GoogleCloudRequestType.TRANSLATE:
                    {
                        if (TranslateSuccessEvent != null)
                            TranslateSuccessEvent(JsonConvert.DeserializeObject<TranslationResponse>(data), requestId);
                    }
                    break;
                case Enumerators.GoogleCloudRequestType.DETECT_LANGUAGE:
                    {
                        if (DetectLanguageSuccessEvent != null)
                            DetectLanguageSuccessEvent(JsonConvert.DeserializeObject<DetectLanguageResponse>(data), requestId);
                    }
                    break;
                case Enumerators.GoogleCloudRequestType.GET_LANGUAGES:
                    {
                        if (GetLanguagesSuccessEvent != null)
                            GetLanguagesSuccessEvent(JsonConvert.DeserializeObject<LanguagesResponse>(data), requestId);
                    }
                    break;
                default: break;
            }
        }

        private Dictionary<string, string> GetHeaders()
        {
            return new Dictionary<string, string>()
            {
#if UNITY_ANDROID
				{ "X-Android-Package", GeneralConfig.Config.packageName },
				{ "X-Android-Cert", GeneralConfig.Config.keySignature }
#endif
            };
        }
    }
}