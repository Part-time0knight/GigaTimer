using App.Model.APIService.TimeAPI;

namespace App.Domain.Factories
{
    public interface IAPIFactory
    {
        TAPI Create<TAPI>()
            where TAPI : class, ITimeAPI;
    }
}