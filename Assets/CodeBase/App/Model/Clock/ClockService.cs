using App.Model.APIService;
using Cysharp.Threading.Tasks;
using System;
using System.Timers;
using UnityEngine;
using Zenject;

namespace App.Model.Clock
{
    public class ClockService : IInitializable, IDisposable, ITickable
    {
        public DateTime Time => _currentTime;

        public event Action InvokeTimeUpdate;

        private readonly TimeAPIService _timeService;
        private readonly Timer _timer;
        private readonly Timer _timerUpdate;
        private readonly Settings _settings;
        private readonly PauseController _pauseController;

        private DateTime _currentTime;

        private bool _needUpdate = false;
        private bool _ticUpdate = false;

        public ClockService(TimeAPIService timeAPIService, PauseController pause, Settings settings) 
        {
            _timeService = timeAPIService;
            _timer = new(interval: 1000d);
            _settings = settings;
            _timerUpdate = new(interval: _settings.TimeOnlineUpdate);
            _pauseController = pause;

        }

        public void Initialize()
        {
            _currentTime = DateTime.Now;
            _pauseController.InvokePause += UpdateAfterPause;
            _timeService.GetTime().ContinueWith(
                (time) => {
                    _currentTime = time;
                    Debug.Log("Initialize load time: " + _currentTime);
                });
            Debug.Log("Initialize: " + _currentTime);
            InitializeTimers();
        }

        public void Dispose()
        {
            _pauseController.InvokePause -= UpdateAfterPause;
            _timer.Stop();
            _timerUpdate.Stop();
        }

        public void Tick()
        {
            if (_needUpdate)
            {
                _needUpdate = false;
                _timeService.GetTime().ContinueWith((time) => _currentTime = time);
            }
            else if (_ticUpdate)
            {
                _ticUpdate = false;
                _currentTime = _currentTime.AddSeconds(value: 1f);
                InvokeTimeUpdate?.Invoke();
            }
        }

        private void InitializeTimers()
        {
            _timer.Elapsed += Tic;
            _timer.Start();
            _timerUpdate.Elapsed += UpdateTime;
            _timerUpdate.Start();
        }

        private void Tic(object source, ElapsedEventArgs e)
        {
            _ticUpdate = true;
        }

        private void UpdateTime(object source, ElapsedEventArgs e)
        {
            _needUpdate = true;
        }

        private void UpdateAfterPause(bool pause)
        {
            if (pause)
                return;
            _timeService.GetTime().ContinueWith((time) => _currentTime = time);
            InvokeTimeUpdate?.Invoke();
        }

        [Serializable]
        public class Settings
        {
            [field: SerializeField] public long TimeOnlineUpdate { get; private set; }
        }
    }
}