using App.Model.StaticData;
using Cysharp.Threading.Tasks;
using System;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

namespace App.Model.APIService.TimeAPI
{
    public class WorldTimeAPI : AbstractTimeAPI
    {
        private DateTime _currentData;

        public WorldTimeAPI(Settings settings) : base(settings) 
        { 
        }

        protected override async UniTask GetTimeStamp()
        {
            UnityWebRequest webRequest;
            webRequest = UnityWebRequest.Get(APIURL.WorldTimeURL);

            await webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                _error = true;
                return;
            }
            XmlDocument data = new();
            data.LoadXml(webRequest.downloadHandler.text);
            XmlNodeList root = data.DocumentElement.FirstChild.ChildNodes;
            foreach (XmlNode node in root)
            {

                // time: 2024-09-05 21:10
                if (node.Name == "time")
                {
                    _currentData = DateTime.ParseExact(node.InnerText, 
                        "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                    return;
                }
            }
        }

        protected override DateTime CalculateData()
            => _currentData.AddMilliseconds(DateTimeOffset.Now.Offset.TotalMilliseconds);

        [Serializable]
        public class Settings : AbstractSettings
        {

        }
    }
}