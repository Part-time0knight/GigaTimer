using App.Model.StaticData;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.Networking;

namespace App.Model.APIService.TimeAPI
{
    public class YandexAPI : AbstractTimeAPI
    {
        private UnityWebRequest _webRequest;

        public YandexAPI(Settings settings) : base(settings)
        {
        }

        protected override async UniTask GetTimeStamp()
        {
            _webRequest = UnityWebRequest.Get(APIURL.YandexURL);
            await _webRequest.SendWebRequest();
            if (_webRequest.result != UnityWebRequest.Result.Success)
            {
                _error = true;
                _webRequest.Dispose();
                return;
            }

            _timestamp = Convert.ToInt64(_webRequest.downloadHandler.text);
            _webRequest.Dispose();
        }

        [Serializable]
        public class Settings : AbstractSettings
        {

        }
    }


}