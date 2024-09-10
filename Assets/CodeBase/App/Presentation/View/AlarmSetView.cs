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
    public class AlarmSetView : AbstractPayloadView<AlarmSetViewModel>
    {
        [field: SerializeField] private TMP_Text _clockText;
        [field: SerializeField] private Hand _secondHand;
        [field: SerializeField] private Hand _minuteHand;
        [field: SerializeField] private Hand _hourHand;
        [field: SerializeField] private Button _backButton;
        [field: SerializeField] private Button _acceptButton;
        [field: SerializeField] private Toggle _AMToggle;
        [field: SerializeField] private Toggle _PMToggle;

        [Inject]
        protected override void Construct(AlarmSetViewModel viewModel)
        {
            base.Construct(viewModel);
            _secondHand.Initialize(AlarmSetViewModel.Hand.Second, OnDrag);
            _minuteHand.Initialize(AlarmSetViewModel.Hand.Minute, OnDrag);
            _hourHand.Initialize(AlarmSetViewModel.Hand.Hour, OnDrag);
            _viewModel.InvokeInitTime += OnOpenUpdate;
            _viewModel.InvokeUpdateTime += OnAlarmChange;
            _backButton.onClick.AddListener(_viewModel.InvokeClose);
            _acceptButton.onClick.AddListener(_viewModel.AcceptAlarm);

            _AMToggle.onValueChanged.AddListener(
                (value) => OnFormatChange(AlarmSetViewModel.Format.AM, value));
            _PMToggle.onValueChanged.AddListener(
                (value) => OnFormatChange(AlarmSetViewModel.Format.PM, value));
        }

        private void OnFormatChange(AlarmSetViewModel.Format format, bool active)
        {
            if (!active)
                return;
            _viewModel.ChangeFormat(format);
        }

        private void OnOpenUpdate(AlarmDto dto)
        {
            SetDto(dto);
        }

        private void OnAlarmChange(AlarmDto dto)
        {
            SetDto(dto);
        }

        private void SetDto(AlarmDto dto)
        {
            _clockText.text = dto.TimerText;
            _secondHand.SetAngle(dto.Hands[AlarmSetViewModel.Hand.Second]);
            _minuteHand.SetAngle(dto.Hands[AlarmSetViewModel.Hand.Minute]);
            _hourHand.SetAngle(dto.Hands[AlarmSetViewModel.Hand.Hour]);
            switch (dto.Format)
            {
                case AlarmSetViewModel.Format.AM:
                    _AMToggle.isOn = true;
                    break;
                case AlarmSetViewModel.Format.PM:
                    _PMToggle.isOn = true;
                    break;
            }
        }

        private void OnDrag(AlarmSetViewModel.Hand hand, Vector2 handWorldPosition)
        {
            _viewModel.ReadDrag(hand, handWorldPosition);
        }

        private void OnDestroy()
        {
            _viewModel.InvokeInitTime -= OnOpenUpdate;
            _viewModel.InvokeUpdateTime -= OnAlarmChange;
            _backButton.onClick.RemoveListener(_viewModel.InvokeClose);
            _acceptButton.onClick.RemoveListener(_viewModel.AcceptAlarm);
            _AMToggle.onValueChanged.RemoveAllListeners();
            _PMToggle.onValueChanged.RemoveAllListeners();
        }
    }
}