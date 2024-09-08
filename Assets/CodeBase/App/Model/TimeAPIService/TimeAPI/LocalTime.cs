using Cysharp.Threading.Tasks;
using System;

namespace App.Model.TimeAPIService.TimeAPI
{
    public class LocalTime : ITimeAPI
    {
        public int Priority { get; }

        public LocalTime()
        {
            Priority = 10;
        }

        public async UniTask<DateTime> Get()
        {
            return DateTime.Now;
        }
    }
}