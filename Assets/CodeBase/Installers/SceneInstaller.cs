using App.Domain.Factories;
using App.Model.Clock;
using App.Model.APIService;
using Zenject;

namespace Installers
{
    public class SceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallFactories();
            InstallServices();
        }

        private void InstallServices()
        {
            Container.BindInterfacesAndSelfTo<TimeAPIService>().AsSingle();
            Container.BindInterfacesAndSelfTo<Clock>().AsSingle();
        }

        private void InstallFactories()
        {
            Container.BindInterfacesTo<APIFactory>().AsSingle();
        }
    }
}