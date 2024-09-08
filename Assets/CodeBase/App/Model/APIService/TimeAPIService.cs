using App.Domain.Factories;
using App.Model.APIService.TimeAPI;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace App.Model.APIService
{
    public class TimeAPIService : IInitializable
    {
        private readonly List<ITimeAPI> _timeAPIs;
        private readonly IAPIFactory _factory;

        public TimeAPIService(IAPIFactory factory) 
        {
            _timeAPIs = new();
            _factory = factory;
        }

        public void Initialize()
        {
            ResolveAPI();
        }

        public async UniTask<DateTime> GetTime()
        {
            int lowestIndex = int.MaxValue;
            DateTime result = DateTime.MinValue;
            foreach (var api in _timeAPIs) 
            {
                if (api.Priority < lowestIndex)
                { 
                    result = await api.Get();
                    lowestIndex = api.Priority;
                }
            }
            return result;
        }

        private void ResolveAPI()
        {
            _timeAPIs.Add(_factory.Create<FirebaseAPI>());
            _timeAPIs.Add(_factory.Create<LocalTime>());
            _timeAPIs.Add(_factory.Create<WorldTimeAPI>());
            _timeAPIs.Add(_factory.Create<YandexAPI>());
        }
    }
}