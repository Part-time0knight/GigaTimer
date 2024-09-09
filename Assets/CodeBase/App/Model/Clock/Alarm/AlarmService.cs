using App.Domain.Pools;
using System;

using Zenject;

namespace App.Model.Clock.Alarm
{
    public class AlarmService : IInitializable, IDisposable
    {
        public event Action InvokeAlarm;

        private readonly AlarmsPool _pool;
        private readonly Clock _clock;

        private IAlarm _currentAlarm;


        public AlarmService(AlarmsPool pool, Clock clock)
        {
            _pool = pool;
            _clock = clock;
        }

        public void Start(DateTime finish)
        {

            _currentAlarm = 
                _pool.Spawn()
                .Start(_clock.Time, finish)
                .SetCallback(OnAlarmFinish);
            
        }

        private void OnAlarmFinish()
        {
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