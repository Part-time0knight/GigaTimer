using App.Model.StaticData;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.Networking;

namespace App.Model.TimeAPIService.TimeAPI
{
    public class YandexAPI : AbstractTimeAPI
    {
        protected override async UniTask GetTimeStamp()
        {
            UnityWebRequest webRequest;
            webRequest = UnityWebRequest.Get(APIURL.YandexURL);

            await webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                _error = true;
                return;
            }

            _timestamp = Convert.ToInt64(webRequest.downloadHandler.text);
        }
    }
}