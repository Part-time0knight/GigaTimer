using System;

namespace App.Model.Clock.Alarm
{
    public interface IAlarm
    {
        IAlarm Start(DateTime currentTime, DateTime endTime);

        void SetCallback(Action callback);

        void Update(DateTime currentTime);

        void Stop();
    }
}