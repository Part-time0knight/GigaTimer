using App.Domain.Factories;
using App.Model.Clock;
using App.Model.APIService;
using Zenject;
using Core.MVVM.Windows;
using App.Infrastructure;
using App.Presentation.ViewModel;
using System;
using UnityEngine;
using App.Model.Clock.Alarm;
using App.Domain.Pools;

namespace Installers
{
    public class SceneInstaller : MonoInstaller
    {
        [field: SerializeField] private Settings settings;

        public override void InstallBindings()
        {
            InstallFactories();
            InstallPools();
            InstallServices();
            InstallViewModels();
        }

        private void InstallServices()
        {
            Container.Bind<PauseController>()
                .FromComponentInNewPrefab(settings.Pause).AsSingle();

            Container.BindInterfacesAndSelfTo<TimeAPIService>()
                .AsSingle()
                .NonLazy();

            Container
                .BindInterfacesAndSelfTo<Clock>()
                .AsSingle()
                .NonLazy();

            Container
                .BindInterfacesAndSelfTo<WindowFsm>()
                .AsSingle()
                .NonLazy();

            Container
                .BindInterfacesAndSelfTo<GameFsm>()
                .AsSingle()
                .NonLazy();

            Container
                .BindInterfacesAndSelfTo<AlarmService>()
                .AsSingle()
                .NonLazy();
        }

        private void InstallViewModels()
        {
            Container
                .BindInterfacesAndSelfTo<ClockViewModel>()
                .AsSingle()
                .NonLazy();
        }

        private void InstallFactories()
        {
            Container.BindInterfacesTo<APIFactory>().AsSingle();
            Container.BindInterfacesTo<StatesFactory>().AsSingle();
        }

        private void InstallPools()
        {
            Container.BindMemoryPool<Alarm, AlarmsPool>().WithInitialSize(size: 1);
        }

        [Serializable]
        public class Settings
        {
            public PauseController Pause;
        }
    }
}