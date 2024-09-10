using App.Domain.Dto;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace App.Presentation.Elements
{
    public class Clock : MonoBehaviour
    {
        [field: SerializeField] private Image _secondHandImage;
        [field: SerializeField] private Image _minuteHandImage;
        [field: SerializeField] private Image _hourHandImage;

        public void Set(ClockDto dto)
        {
            _secondHandImage.transform.DOKill();
            _secondHandImage.transform.DOLocalRotate(new(0f, 0f, dto.SecondHandAngle), duration: 0.8f);
            _minuteHandImage.transform.DOKill();
            _minuteHandImage.transform.DOLocalRotate(new(0f, 0f, dto.MinuteHandAngle), duration: 0.8f);
            _hourHandImage.transform.DOKill();
            _hourHandImage.transform.DOLocalRotate(new(0f, 0f, dto.HourHandAngle), duration: 0.8f);
        }
    }
}