using App.Domain.Dto;
using App.Model.Clock;
using App.Model.Clock.Alarm;
using App.Presentation.View;
using Core.MVVM.ViewModel;
using Core.MVVM.Windows;
using System;
using UnityEngine;

namespace App.Presentation.ViewModel
{
    public class AlarmSetViewModel : AbstractViewModel
    {
        public event Action<AlarmDto> InvokeInitTime;
        public event Action<AlarmDto> InvokeUpdateTime;

        private readonly AlarmService _alarm;
        private readonly Clock _clock;
        private readonly AlarmDto _alarmDto;

        private DateTime _setTime;

        private Vector2 _mousePosition;
        private float _angle;

        private int _hour;
        private int _minute;
        private int _second;

        private string _hourString;
        private string _minuteString;
        private string _secondString;

        protected override Type Window => typeof(AlarmSetView);

        public AlarmSetViewModel(IWindowFsm windowFsm, AlarmService alarm, Clock clock) : base(windowFsm)
        {
            _alarm = alarm;
            _clock = clock;
            _alarmDto = new();
        }

        public override void InvokeClose()
        {
            _windowFsm.CloseWindow();
        }

        public override void InvokeOpen()
        {
            _windowFsm.OpenWindow(Window, inHistory: true);
        }

        /// <param name="hand"></param>
        /// <param name="handCenterPoint">World position of hand.</param>
        public void ReadDrag(Hand hand, Vector2 handCenterPoint)
        {
            _mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - handCenterPoint;
            _angle = -(Vector2.Angle(_mousePosition, Vector2.up) * Mathf.Sign(_mousePosition.x));
            if (_angle < 0f)
                _angle += 360f;
            _alarmDto.Hands[hand] = _angle;
            TimeToText();
            InvokeUpdateTime?.Invoke(_alarmDto);
        }

        public void ChangeFormat(Format format)
        {
            _alarmDto.Format = format;
            TimeToText();
            InvokeUpdateTime?.Invoke(_alarmDto);
        }

        public void AcceptAlarm()
        {
            _hour = Mathf.FloorToInt((_alarmDto.Hands[Hand.Hour] - 360) / -30f) % 12;
            _minute = Mathf.FloorToInt((_alarmDto.Hands[Hand.Minute] - 360) / -6f) % 60;
            _second = Mathf.FloorToInt((_alarmDto.Hands[Hand.Second] - 360) / -6f) % 60;
            _setTime = new(
                year: _setTime.Year,
                month: _setTime.Month,
                day: _setTime.Day,
                hour: _hour,
                minute: _minute,
                second: _second);
            while(_setTime < _clock.Time)
                _setTime = _setTime.AddDays(1f);
            _alarm.Start(_setTime);

            InvokeClose();
        }

        protected override void HandleOpenedWindow(Type uiWindow)
        {
            base.HandleOpenedWindow(uiWindow);
            OpenTime();
        }

        private void TimeToText()
        {
            _hour = Mathf.FloorToInt((_alarmDto.Hands[Hand.Hour] - 360) / -30f) % 12;
            _minute = Mathf.FloorToInt((_alarmDto.Hands[Hand.Minute] - 360) / -6f) % 60;
            _second = Mathf.FloorToInt((_alarmDto.Hands[Hand.Second] - 360) / -6f) % 60;

            if (_alarmDto.Format == Format.PM)
                _hour += 12; 

            if (_hour < 10)
                _hourString = $"0{_hour}";
            else
                _hourString = _hour.ToString();

            if (_minute < 10)
                _minuteString = $"0{_minute}";
            else
                _minuteString = _minute.ToString();

            if (_second < 10)
                _secondString = $"0{_second}";
            else
                _secondString = _second.ToString();

            _alarmDto.TimerText = $"{_hourString}:{_minuteString}:{_secondString}";
        }

        private void OpenTime()
        {
            _setTime = _clock.Time;

            if (_clock.Time.Hour < 10)
                _alarmDto.TimerText = "0" + _clock.Time.ToLongTimeString();
            else
                _alarmDto.TimerText = _clock.Time.ToLongTimeString();

            if (_clock.Time.Hour < 12)
                _alarmDto.Format = Format.AM;
            else
                _alarmDto.Format = Format.PM;

                _alarmDto.Hands.Clear();
            _alarmDto.Hands.Add(Hand.Second, -6f * _clock.Time.Second);
            _alarmDto.Hands.Add(Hand.Minute, -6f * _clock.Time.Minute);
            _alarmDto.Hands.Add(Hand.Hour, -30f * (_clock.Time.Hour % 12));
            InvokeInitTime?.Invoke(_alarmDto);
        }

        public enum Hand
        {
            Second = 0,
            Minute = 1,
            Hour = 2,
        }

        public enum Format
        {
            AM = 0,
            PM = 1,
        }
    }
}