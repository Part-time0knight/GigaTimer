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
        private readonly ClockService _clock;
        private readonly AlarmDto _alarmDto;

        private DateTime _setTime;

        private Vector2 _mousePosition;
        private float _angle;

        private int _hour;
        private int _minute;
        private int _second;

        protected override Type Window => typeof(AlarmSetView);

        public AlarmSetViewModel(IWindowFsm windowFsm, AlarmService alarm, ClockService clock) : base(windowFsm)
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
            _alarm.InvokeAlarm += OnAlarmFinish;


            InvokeClose();
            _windowFsm.OpenWindow(typeof(AlarmTicView), inHistory: true);
        }

        public void ReadInput(Hand hand, string timeText)
        {
            if (timeText == null || timeText == "")
                return;
            switch(hand)
            {
                case Hand.Second:
                    _second = Convert.ToInt32(timeText);
                    if (_second > 60)
                        _second = 60;
                    else if (_second < 0)
                        _second = 0;

                    if (_second < 10)
                        _alarmDto.SecondText = $"0{_second}";
                    else
                        _alarmDto.SecondText = _second.ToString();
                    _alarmDto.Hands[Hand.Second] = -6f * _second;
                    break;

                case Hand.Minute:
                    _minute = Convert.ToInt32(timeText);
                    if (_minute > 60)
                        _minute = 60;
                    else if (_minute < 0)
                        _minute = 0;

                    if (_minute < 10)
                        _alarmDto.MinuteText = $"0{_minute}";
                    else
                        _alarmDto.MinuteText = _minute.ToString();

                    _alarmDto.Hands[Hand.Minute] = -6f * _minute;
                        break;

                case Hand.Hour:
                    _hour = Convert.ToInt32(timeText);
                    if (_alarmDto.Format == Format.AM
                        && _hour >= 12)
                        _hour = 11;
                    else if (_alarmDto.Format == Format.PM)
                    {
                        if (_hour >= 24)
                            _hour = 23;
                        else if (_hour < 12)
                            _hour += 12;
                    }
                    
                    if (_hour < 0)
                        _hour = 0;

                    if (_hour < 10)
                        _alarmDto.HourText = $"0{_hour}";
                    else
                        _alarmDto.HourText = _hour.ToString();

                    _alarmDto.Hands[Hand.Hour] = -30f * (_hour % 12);
                    break;
            }
            InvokeUpdateTime?.Invoke(_alarmDto);
        }

        protected override void HandleOpenedWindow(Type uiWindow)
        {
            base.HandleOpenedWindow(uiWindow);
            if (uiWindow != Window)
                return;
            OpenTime();
        }

        private void OnAlarmFinish()
        {
            _alarm.InvokeAlarm -= OnAlarmFinish;
            _windowFsm.OpenWindow(typeof(AlarmFinishView), inHistory: true);
        }

        private void TimeToText()
        {
            _hour = Mathf.FloorToInt((_alarmDto.Hands[Hand.Hour] - 360) / -30f) % 12;
            _minute = Mathf.FloorToInt((_alarmDto.Hands[Hand.Minute] - 360) / -6f) % 60;
            _second = Mathf.FloorToInt((_alarmDto.Hands[Hand.Second] - 360) / -6f) % 60;

            if (_alarmDto.Format == Format.PM)
                _hour += 12; 

            if (_hour < 10)
                _alarmDto.HourText = $"0{_hour}";
            else
                _alarmDto.HourText = _hour.ToString();

            if (_minute < 10)
                _alarmDto.MinuteText = $"0{_minute}";
            else
                _alarmDto.MinuteText = _minute.ToString();

            if (_second < 10)
                _alarmDto.SecondText = $"0{_second}";
            else
                _alarmDto.SecondText = _second.ToString();
        }

        private void OpenTime()
        {
            _setTime = _clock.Time;

            if (_clock.Time.Hour < 10)
                _alarmDto.HourText = $"0{_clock.Time.Hour}";
            else
                _alarmDto.HourText = _clock.Time.Hour.ToString();

            if (_clock.Time.Minute < 10)
                _alarmDto.MinuteText = $"0{_clock.Time.Minute}";
            else
                _alarmDto.MinuteText = _clock.Time.Minute.ToString();

            if (_clock.Time.Second < 10)
                _alarmDto.SecondText = $"0{_clock.Time.Second}";
            else
                _alarmDto.SecondText = _clock.Time.Second.ToString();

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