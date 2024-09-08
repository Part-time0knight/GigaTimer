using App.Model.TimeAPIService;
using Zenject;

namespace Installers
{
    public class SceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<TimeAPIService>().AsSingle();
        }
    }
}