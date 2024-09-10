using App.Presentation.View;
using Core.Infrastructure.GameFsm;
using Core.Infrastructure.GameFsm.States;
using Core.MVVM.Windows;

namespace App.Infrastructure
{
    public class InitializeState : AbstractState, IState
    {
        private readonly IWindowResolve _windowResolve;

        public InitializeState(IGameStateMachine gameStateMachine,
            IWindowResolve windowResolve) : base(gameStateMachine)
        {
            _windowResolve = windowResolve;
        }

        public void OnEnter()
        {
            WindowResolve();
            GameStateMachine.Enter<MainState>();
        }

        public override void OnExit()
        {
            
        }

        private void WindowResolve()
        {
            _windowResolve.CleanUp();
            _windowResolve.Set<ClockView>();
            _windowResolve.Set<AlarmSetView>();
            _windowResolve.Set<AlarmTicView>();
            _windowResolve.Set<AlarmFinishView>();
        }
    }
}