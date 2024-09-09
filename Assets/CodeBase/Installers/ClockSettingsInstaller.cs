using App.Model.APIService.TimeAPI;
using App.Model.Clock;
using System;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(fileName = "ClockSettingsInstaller", menuName = "Installers/ClockSettingsInstaller")]
    public class ClockSettingsInstaller : ScriptableObjectInstaller<ClockSettingsInstaller>
    {

        [field: SerializeField] private APISettings _apiSettings;
        [field: SerializeField] private Clock.Settings _clockSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(_apiSettings.YandexSettings).AsSingle();
            Container.BindInstance(_apiSettings.WorldTimeSettings).AsSingle();
            Container.BindInstance(_apiSettings.FirebaseSettings).AsSingle();
            Container.BindInstance(_clockSettings).AsSingle();
        }

        [Serializable]
        public class APISettings
        {
            public YandexAPI.Settings YandexSettings;
            public WorldTimeAPI.Settings WorldTimeSettings;
            public FirebaseAPI.Settings FirebaseSettings;
        }
    }
}