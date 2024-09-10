using App.Model.Clock.Alarm;
using App.Presentation.View;
using Core.Infrastructure.GameFsm;
using Core.Infrastructure.GameFsm.States;
using Core.MVVM.Windows;

namespace App.Infrastructure
{
    public class MainState : AbstractState, IState
    {
        private readonly IWindowFsm _windowFsm;
        private readonly AlarmService _alarmService;

        public MainState(IGameStateMachine gameStateMachine,
            IWindowFsm windowFsm,
            AlarmService alarmService) : base(gameStateMachine)
        {
            _windowFsm = windowFsm;
            _alarmService = alarmService;
            _alarmService.InvokeAlarm += OnAlarmFinish;
        }

        public void OnEnter()
        {
            _windowFsm.OpenWindow(typeof(ClockView), inHistory: true);
            _alarmService.InvokeAlarm += OnAlarmFinish;
        }

        public override void OnExit()
        {
            _alarmService.InvokeAlarm -= OnAlarmFinish;
        }

        private void OnAlarmFinish()
        {
            _windowFsm.OpenWindow(typeof(AlarmFinishView), inHistory: true);
        }
    }
}