using Core.Domain.Factories;
using Core.Infrastructure.GameFsm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace App.Infrastructure
{
    public class GameFsm : AbstractGameStateMachine, IInitializable
    {
        public GameFsm(IStatesFactory factory) : base(factory)
        {
        }

        public void Initialize()
        {
            StateResolve();
            Enter<InitializeState>();
        }

        private void StateResolve()
        {
            _states.Add(typeof(InitializeState), _factory.Create<InitializeState>());
            _states.Add(typeof(MainState), _factory.Create<MainState>());
        }
    }
}