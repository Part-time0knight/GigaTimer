using App.Domain.Dto;
using App.Model.Clock;
using App.Presentation.View;
using Core.MVVM.ViewModel;
using Core.MVVM.Windows;
using System;

namespace App.Presentation.ViewModel
{
    public class ClockViewModel : AbstractViewModel
    {
        public event Action<ClockDto> InvokeTimeUpdate;

        private readonly Clock _clock;
        private readonly ClockDto _dto;

        private string _time;

        protected override Type Window => typeof(ClockView);

        public ClockViewModel(IWindowFsm windowFsm, Clock clock) : base(windowFsm)
        {
            _clock = clock;
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

        protected override void HandleOpenedWindow(Type uiWindow)
        {
            base.HandleOpenedWindow(uiWindow);
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

            _time = _clock.Time.ToLongTimeString();
            if (_clock.Time.Hour < 10)
                _time = "0" + _clock.Time.ToLongTimeString();
            else
                _time = _clock.Time.ToLongTimeString();
            _dto.ClockText = _time;

            _dto.SecondHandAngle = -6f * _clock.Time.Second; //-360 * _clock.Time.Second/60
            _dto.MinuteHandAngle = -6f * _clock.Time.Minute; //-360 * _clock.Time.Minute/60
            _dto.HourHandAngle = -30f * (_clock.Time.Hour % 12); 

            InvokeTimeUpdate?.Invoke(_dto);
        }
    }
}