using System;
using System.Timers;

namespace App.Model.Clock.Alarm
{
    public class Alarm : IAlarm
    {
        private Action _callbacks;

        private DateTime _currentTime;
        private DateTime _endTime;
        
        private Timer _timer;

        public void SetCallback(Action callback)
        {
            _callbacks += callback;
        }

        public IAlarm Start(DateTime currentTime, DateTime endTime)
        {
            _currentTime = currentTime;
            _endTime = endTime;
            _timer = new Timer(interval: 1000d);
            _timer.Elapsed += SecondTic;
            _timer.Start();
            return this;
        }

        public void Stop()
        {
            _timer.Stop();
            _callbacks = null;
        }

        public void Update(DateTime currentTime)
        {
            _currentTime = currentTime;
            if (_currentTime >= _endTime)
                OnFinish();
        }

        private void SecondTic(object source, ElapsedEventArgs e)
        { 
            _currentTime = _currentTime.AddSeconds(value: 1f);
            if (_currentTime >= _endTime)
                OnFinish();
        }

        private void OnFinish()
        {
            _callbacks?.Invoke();
            Stop();
        }

    }
}