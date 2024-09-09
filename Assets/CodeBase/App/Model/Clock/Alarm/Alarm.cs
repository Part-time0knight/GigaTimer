using System;
using System.Timers;

namespace App.Model.Clock.Alarm
{
    public class Alarm : IAlarm
    {
        private Action _callbacks;

        private DateTime _currentTime;
        private DateTime _endTime;

        public IAlarm SetCallback(Action callback)
        {
            _callbacks += callback;
            return this;
        }

        public IAlarm Start(DateTime currentTime, DateTime endTime)
        {
            _currentTime = currentTime;
            _endTime = endTime;
            return this;
        }

        public IAlarm Stop()
        {
            _callbacks = null;
            return this;
        }

        public IAlarm Update(DateTime currentTime)
        {
            _currentTime = currentTime;
            if (_currentTime >= _endTime)
                OnFinish();
            return this;
        }

        private void OnFinish()
        {
            _callbacks?.Invoke();
        }

    }
}