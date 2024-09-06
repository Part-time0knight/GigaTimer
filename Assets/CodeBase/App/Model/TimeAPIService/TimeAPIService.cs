using App.Model.TimeAPIService.TimeAPI;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace App.Model.TimeAPIService
{
    public class TimeAPIService : IInitializable
    {
        private readonly List<ITimeAPI> _timeAPIs;

        public TimeAPIService() 
        {
            _timeAPIs = new();
        }

        public void Initialize()
        {
            ResolveAPI();
            GetTime();
        }

        public async UniTask GetTime()
        {
            int lowestIndex = 11;
            DateTime result;
            foreach (var api in _timeAPIs) 
            {
                if (api.Priority < lowestIndex)
                {
                    lowestIndex = api.Priority;
                    result = await api.Get();
                    Debug.Log("priority: " + lowestIndex + " | Data:" + result);
                }
            }

        }

        private void ResolveAPI()
        {
            _timeAPIs.Add(new LocalTime());
            _timeAPIs.Add(new WorldTimeAPI());
            _timeAPIs.Add(new YandexAPI());
        }
    }
}