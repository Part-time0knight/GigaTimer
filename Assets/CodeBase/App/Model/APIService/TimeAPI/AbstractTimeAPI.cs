using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace App.Model.APIService.TimeAPI
{
    public abstract class AbstractTimeAPI : ITimeAPI
    {
        public int Priority { get; set; }

        protected readonly AbstractSettings _settings;

        protected long _timestamp;
        protected bool _error = false;

        public AbstractTimeAPI(AbstractSettings settings)
        {
            _settings = settings;
            Priority = _settings.Priority;
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

    [Serializable]
    public abstract class AbstractSettings
    {
        [field: SerializeField] public int Priority { get; private set; }
    }
}