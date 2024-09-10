using App.Domain.Pools;
using System;

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
            
        }

        public void Stop()
        {
            _currentAlarm.Stop();
            _active = false;
        }

        private void OnAlarmFinish()
        {
            _active = false;
            _pool.Despawn(_currentAlarm.Stop());
            InvokeAlarm?.Invoke();
        }

        private void OnTimeUpdate()
        {
            _currentAlarm?.Update(_clock.Time);
        }

        public void Initialize()
        {
            _clock.InvokeTimeUpdate += OnTimeUpdate;
        }

        public void Dispose()
        {
            _clock.InvokeTimeUpdate -= OnTimeUpdate;
        }
    }
}