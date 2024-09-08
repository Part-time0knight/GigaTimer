using App.Model.APIService;
using System;
using System.Timers;
using UnityEngine;
using Zenject;

namespace App.Model.Clock
{
    public class Clock : IInitializable, IDisposable, ITickable
    {
        private readonly TimeAPIService _timeService;
        private readonly Timer _timer;
        private readonly Timer _timerUpdate;
        private readonly Settings _settings;

        private DateTime _currentTime;

        private bool _needUpdate = false;

        public Clock(TimeAPIService timeAPIService, Settings settings) 
        {
            _timeService = timeAPIService;
            _timer = new(interval: 1000);
            _settings = settings;
            _timerUpdate = new(interval: _settings.TimeOnlineUpdate);

        }

        public async void Initialize()
        {
            _currentTime = await _timeService.GetTime();
            Debug.Log("Initialize: " + _currentTime);
            InitializeTimers();
        }

        private void InitializeTimers()
        {
            _timer.Elapsed += Tic;
            _timerUpdate.Elapsed += UpdateTime;
            _timerUpdate.Start();
        }

        private void Tic(object source, ElapsedEventArgs e)
        {
            _currentTime.AddSeconds(value: 1f);
        }

        private void UpdateTime(object source, ElapsedEventArgs e)
        {
            _needUpdate = true;
        }

        public void Dispose()
        {
            _timer.Stop();
            _timerUpdate.Stop();
        }

        public async void Tick()
        {
            if (_needUpdate)
            {
                _needUpdate = false;
                _currentTime = await _timeService.GetTime();
                Debug.Log("Update: " + _currentTime);
            }
        }

        [Serializable]
        public class Settings
        {
            [field: SerializeField] public long TimeOnlineUpdate { get; private set; }
        }
    }
}