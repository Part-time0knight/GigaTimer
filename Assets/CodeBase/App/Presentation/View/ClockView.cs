using App.Domain.Dto;
using App.Presentation.ViewModel;
using Core.MVVM.View;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Presentation.View
{
    public class ClockView : AbstractPayloadView<ClockViewModel>
    {
        [field: SerializeField] private TMP_Text _clockText;
        [field: SerializeField] private Image _secondHandImage;
        [field: SerializeField] private Image _minuteHandImage;

        [Inject]
        protected override void Construct(ClockViewModel viewModel)
        {
            base.Construct(viewModel);
            _viewModel.InvokeTimeUpdate += TimeUpdate;
        }

        private void TimeUpdate(ClockDto dto)
        {
            _clockText.text = dto.ClockText;
            _secondHandImage.transform.DOKill();
            _secondHandImage.transform.DOLocalRotate(new(0f, 0f, dto.SecondHandAngle), duration: 0.6f);
            _minuteHandImage.transform.DOKill();
            _minuteHandImage.transform.DOLocalRotate(new(0f, 0f, dto.MinuteHandAngle), duration: 0.6f);
        }

        private void OnDestroy()
        {
            _viewModel.InvokeTimeUpdate -= TimeUpdate;
        }
    }
}