using Cysharp.Threading.Tasks;
using System;

namespace App.Model.APIService.TimeAPI
{
    public class LocalTime : ITimeAPI
    {
        public int Priority { get; }

        public LocalTime()
        {
            Priority = int.MaxValue - 1;
        }

        public async UniTask<DateTime> Get()
        {
            return DateTime.Now;
        }
    }
}