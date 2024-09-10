using App.Domain.Dto;
using System;
using TMPro;
using UnityEngine;

namespace App.Presentation.Elements
{
    public class InputTime : MonoBehaviour
    {

        public event Action<string> InvokeInputHours;
        public event Action<string> InvokeInputMinutes;
        public event Action<string> InvokeInputSeconds;

        [field: SerializeField] private TMP_InputField _hourInput;
        [field: SerializeField] private TMP_InputField _minuteInput;
        [field: SerializeField] private TMP_InputField _secondInput;

        public void SetText(AlarmDto dto)
        {
            _hourInput.text = dto.HourText;
            _minuteInput.text = dto.MinuteText;
            _secondInput.text = dto.SecondText;
        }

        private void Awake()
        {
            _hourInput.onEndEdit.AddListener((value) => InvokeInputHours?.Invoke(value));
            _minuteInput.onEndEdit.AddListener((value) => InvokeInputMinutes?.Invoke(value));
            _secondInput.onEndEdit.AddListener((value) => InvokeInputSeconds?.Invoke(value));
        }

        private void OnDestroy()
        {
            _hourInput.onEndEdit.RemoveAllListeners();
            _minuteInput.onEndEdit.RemoveAllListeners();
            _secondInput.onEndEdit.RemoveAllListeners();
        }
    }
}