using Cysharp.Threading.Tasks;
using Firebase.Database;
using Newtonsoft.Json;
using System;

namespace App.Model.APIService.TimeAPI
{
    public class FirebaseAPI : AbstractTimeAPI
    {
        private readonly DatabaseReference _database;

        public FirebaseAPI(Settings settings) : base(settings)
        {
            _database = FirebaseDatabase.DefaultInstance.RootReference;
        }

        protected override async UniTask GetTimeStamp()
        {
            await _database.Child("TimeStamp")
                .SetValueAsync(ServerValue.Timestamp);
            var snapshot = await _database.Child("TimeStamp").GetValueAsync();
            _timestamp = JsonConvert.DeserializeObject<long>(snapshot.GetRawJsonValue());
        }

        [Serializable]
        public class Settings : AbstractSettings
        {

        }
    }
}