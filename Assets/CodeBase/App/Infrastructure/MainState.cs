using App.Presentation.View;
using Core.Infrastructure.GameFsm;
using Core.Infrastructure.GameFsm.States;
using Core.MVVM.Windows;

namespace App.Infrastructure
{
    public class MainState : AbstractState, IState
    {
        private readonly IWindowFsm _windowFsm;

        public MainState(IGameStateMachine gameStateMachine,
            IWindowFsm windowFsm) : base(gameStateMachine)
        {
            _windowFsm = windowFsm;
        }

        public void OnEnter()
        {
            _windowFsm.OpenWindow(typeof(ClockView), inHistory: true);
        }

        public override void OnExit()
        {

        }
    }
}