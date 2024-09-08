using Cysharp.Threading.Tasks;
using System;

namespace App.Model.APIService.TimeAPI
{
    public interface ITimeAPI
    {
        int Priority { get; }

        UniTask<DateTime> Get();
    }
}