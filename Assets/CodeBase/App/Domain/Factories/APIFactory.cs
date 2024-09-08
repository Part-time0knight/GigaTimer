using App.Model.APIService.TimeAPI;
using Zenject;

namespace App.Domain.Factories
{
    public class APIFactory : IAPIFactory
    {
        private readonly DiContainer _container;

        public APIFactory(DiContainer container)
        {
            _container = container;
        }

        public TAPI Create<TAPI>() where TAPI : class, ITimeAPI
        {
            TAPI api = _container.Instantiate<TAPI>();
            return api;
        }
    }
}