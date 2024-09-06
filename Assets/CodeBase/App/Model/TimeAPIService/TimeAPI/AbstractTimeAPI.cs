using Cysharp.Threading.Tasks;
using System;

namespace App.Model.TimeAPIService.TimeAPI
{
    public abstract class AbstractTimeAPI : ITimeAPI
    {
        public int Priority { get; set; }

        protected long _timestamp;
        protected bool _error = false;

        public AbstractTimeAPI()
        {
            Priority = 0;
        }

        public virtual async UniTask<DateTime> Get()
        {
            await GetTimeStamp();
            if (_error)
                return DateTime.MinValue;

            return CalculateData();
        }

        protected virtual DateTime CalculateData()
            => new DateTime(1970, 1, 1).AddMilliseconds(_timestamp + DateTimeOffset.Now.Offset.TotalMilliseconds);

        protected abstract UniTask GetTimeStamp();
    }
}