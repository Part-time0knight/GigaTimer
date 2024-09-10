using App.Model.Clock.Alarm;
using App.Model.Clock;
using App.Presentation.View;
using Core.MVVM.ViewModel;
using Core.MVVM.Windows;
using System;
using App.Domain.Dto;
using Core.Data.Dto;
using UnityEngine;


namespace App.Presentation.ViewModel
{
    public class AlarmTicViewModel : AbstractViewModel
    {
        public event Action<string> InvokeAlarmTime;
        public event Action<ClockDto> InvokeTime;


        private readonly AlarmService _alarm;
        private readonly ClockService _clock;
        private readonly ClockDto _dto;

        protected override Type Window => typeof(AlarmTicView);

        public AlarmTicViewModel(IWindowFsm windowFsm, AlarmService alarm, ClockService clock) : base(windowFsm)
        {
            _alarm = alarm;
            _clock = clock;
            _dto = new();
        }

        public void AlarmStop()
        {
            _alarm.Stop();
            InvokeClose();
            _windowFsm.OpenWindow(typeof(AlarmSetView), inHistory: true);
        }

        public override void InvokeClose()
        {
            _windowFsm.CloseWindow();
        }

        public override void InvokeOpen()
        {
            _windowFsm.OpenWindow(Window, inHistory: true);
        }

        protected override void HandleOpenedWindow(Type uiWindow)
        {
            base.HandleOpenedWindow(uiWindow);

            if (uiWindow != Window)
                return;

            if (!_alarm.Active)
            {
                InvokeClose();
                return;
            }
            _clock.InvokeTimeUpdate += Update;
            string time;
            if (_alarm.FinishTime.Hour < 10)
                time = "0" + _alarm.FinishTime.ToLongTimeString();
            else
                time = _alarm.FinishTime.ToLongTimeString();
            Update();
            InvokeAlarmTime?.Invoke(time);
        }

        protected override void HandleClosedWindow(Type uiWindow)
        {
            base.HandleClosedWindow(uiWindow);
            _clock.InvokeTimeUpdate -= Update;
        }

        private void Update()
        {
            ClockConverter.MakeConvert(_dto, _clock.Time);
            InvokeTime?.Invoke(_dto);
        }
    }
}