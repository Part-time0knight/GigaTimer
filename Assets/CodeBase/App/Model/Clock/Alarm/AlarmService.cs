using App.Domain.Pools;
using App.Model.StaticData;
using System;
using UnityEngine;
using Zenject;

namespace App.Model.Clock.Alarm
{
    public class AlarmService : IInitializable, IDisposable
    {
        public event Action InvokeAlarm;

        public DateTime FinishTime => _currentTime;
        public bool Active => _active;

        private readonly AlarmsPool _pool;
        private readonly ClockService _clock;
        private IAlarm _currentAlarm;
        private DateTime _currentTime;
        private DateTime _loadTime;
        private bool _active = false;

        public AlarmService(AlarmsPool pool, ClockService clock)
        {
            _pool = pool;
            _clock = clock;
            _currentTime = DateTime.MinValue;
        }

        public void Start(DateTime finish)
        {
            _currentTime = finish;
            _active = true;
            _currentAlarm = 
                _pool.Spawn()
                .Start(_clock.Time, finish)
                .SetCallback(OnAlarmFinish);
            Save();
        }

        public void Stop()
        {
            _currentAlarm.Stop();
            _active = false;
            Save();
        }

        public void Initialize()
        {
            _clock.InvokeTimeUpdate += OnTimeUpdate;
            bool load = Load();
            if (load)
                Start(_loadTime);
        }

        public void Dispose()
        {
            _clock.InvokeTimeUpdate -= OnTimeUpdate;
        }

        private void Save()
        {
            PlayerPrefs.SetInt(SaveStrings.AlarmActive,
                _active == true ? 1 : 0);
            PlayerPrefs.SetString(SaveStrings.AlarmData, _currentTime.ToString());
        }

        private bool Load()
        {
            bool active = PlayerPrefs.GetInt(SaveStrings.AlarmActive) == 1;
            string date = PlayerPrefs.GetString(SaveStrings.AlarmData);
            if (date != null || date != "")
                _loadTime = DateTime.Parse(date);
            Debug.Log(date);
            return active;
        }

        private void OnAlarmFinish()
        {
            _active = false;
            _pool.Despawn(_currentAlarm.Stop());
            InvokeAlarm?.Invoke();
            Save();
        }

        private void OnTimeUpdate()
        {
            _currentAlarm?.Update(_clock.Time);
        }

    }
}