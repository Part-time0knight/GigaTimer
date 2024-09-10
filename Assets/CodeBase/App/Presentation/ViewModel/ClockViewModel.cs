using App.Domain.Dto;
using App.Model.Clock;
using App.Model.Clock.Alarm;
using App.Presentation.View;
using Core.MVVM.ViewModel;
using Core.MVVM.Windows;
using System;

namespace App.Presentation.ViewModel
{
    public class ClockViewModel : AbstractViewModel
    {
        public event Action<ClockDto> InvokeTimeUpdate;

        private readonly ClockService _clock;
        private readonly AlarmService _alarm;
        private readonly ClockDto _dto;

        protected override Type Window => typeof(ClockView);

        public ClockViewModel(IWindowFsm windowFsm, ClockService clock, AlarmService alarm) : base(windowFsm)
        {
            _clock = clock;
            _alarm = alarm;
            _dto = new();
        }

        public override void InvokeClose()
        {
            _windowFsm.CloseWindow();
        }

        public override void InvokeOpen()
        {
            _windowFsm.OpenWindow(Window, inHistory: true);
        }

        public void OpenAlarmWindow()
        {
            if (_alarm.Active)
                _windowFsm.OpenWindow(typeof(AlarmTicView), inHistory: true);
            else
                _windowFsm.OpenWindow(typeof(AlarmSetView), inHistory: true);
        }

        protected override void HandleOpenedWindow(Type uiWindow)
        {
            base.HandleOpenedWindow(uiWindow);
            if (uiWindow != Window)
                return;
            _clock.InvokeTimeUpdate += Update;
            Update();
        }

        protected override void HandleClosedWindow(Type uiWindow)
        {
            base.HandleClosedWindow(uiWindow);
            _clock.InvokeTimeUpdate -= Update;
        }

        private void Update()
        {
            ClockConverter.MakeConvert(_dto, _clock.Time);
            InvokeTimeUpdate?.Invoke(_dto);
        }
    }
}