using System;
using UnityEngine;

namespace App.Model.Clock
{
    public class PauseController : MonoBehaviour
    {
        public event Action<bool> InvokePause;

        private void OnApplicationPause(bool pause)
        {
            InvokePause?.Invoke(pause);
        }
    }
}