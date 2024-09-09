using System;

namespace App.Model.Clock.Alarm
{
    public interface IAlarm
    {
        IAlarm Start(DateTime currentTime, DateTime endTime);

        IAlarm SetCallback(Action callback);

        IAlarm Update(DateTime currentTime);

        IAlarm Stop();
    }
}