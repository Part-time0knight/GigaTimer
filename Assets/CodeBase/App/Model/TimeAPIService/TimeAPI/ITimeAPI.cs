using Cysharp.Threading.Tasks;
using System;

namespace App.Model.TimeAPIService.TimeAPI
{
    public interface ITimeAPI
    {
        int Priority { get; }

        UniTask<DateTime> Get();
    }
}