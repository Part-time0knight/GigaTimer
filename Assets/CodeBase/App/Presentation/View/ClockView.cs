using App.Domain.Dto;
using App.Presentation.Elements;
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
        [field: SerializeField] private Clock _clock;
        [field: SerializeField] private Button _alarmButton;

        [Inject]
        protected override void Construct(ClockViewModel viewModel)
        {
            base.Construct(viewModel);
            _viewModel.InvokeTimeUpdate += TimeUpdate;
            _alarmButton.onClick.
                AddListener(_viewModel.OpenAlarmWindow);
        }

        private void TimeUpdate(ClockDto dto)
        {
            _clockText.text = dto.ClockText;
            _clock.Set(dto);
        }

        private void OnDestroy()
        {
            _viewModel.InvokeTimeUpdate -= TimeUpdate;
            _alarmButton.onClick.
                RemoveListener(_viewModel.OpenAlarmWindow);
        }
    }
}