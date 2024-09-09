using App.Domain.Factories;
using App.Model.Clock;
using App.Model.APIService;
using Zenject;
using Core.MVVM.Windows;
using App.Infrastructure;
using App.Presentation.ViewModel;

namespace Installers
{
    public class SceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallFactories();
            InstallServices();
            InstallViewModels();
        }

        private void InstallServices()
        {
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
    }
}