using App.Domain.Dto;
using App.Presentation.Elements;
using App.Presentation.ViewModel;
using Core.MVVM.View;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Presentation.View
{
    public class AlarmTicView : AbstractPayloadView<AlarmTicViewModel>
    {
        [field: SerializeField] private TMP_Text _currentTimeText;
        [field: SerializeField] private TMP_Text _alarmTimeText;

        [field: SerializeField] private Button _backButton;
        [field: SerializeField] private Button _cancelButton;

        [field: SerializeField] private Clock _clock;

        [Inject]
        protected override void Construct(AlarmTicViewModel viewModel)
        {
            base.Construct(viewModel);

            _viewModel.InvokeAlarmTime += UpdateAlarmTime;
            _viewModel.InvokeTime += TimeUpdate;
            _backButton.onClick.AddListener(_viewModel.InvokeClose);
            _cancelButton.onClick.AddListener(_viewModel.AlarmStop);
        }

        private void UpdateAlarmTime(string timeString)
        {
            _alarmTimeText.text = timeString;
        }

        private void TimeUpdate(ClockDto dto)
        {
            _currentTimeText.text = dto.ClockText;
            _clock.Set(dto);
        }

        private void OnDestroy()
        {
            _viewModel.InvokeAlarmTime -= UpdateAlarmTime;
            _viewModel.InvokeTime -= TimeUpdate;
        }
    }
}