using App.Domain.Dto;
using App.Presentation.Elements;
using Core.MVVM.View;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Presentation.View
{
    public class AlarmFinishView : AbstractPayloadView<AlarmFinishViewModel>
    {
        [field: SerializeField] private TMP_Text _clockText;
        [field: SerializeField] private Clock _clock;
        [field: SerializeField] private Button _backButton;

        [Inject]
        protected override void Construct(AlarmFinishViewModel viewModel)
        {
            base.Construct(viewModel);
            _backButton.onClick.AddListener(_viewModel.InvokeClose);
            _viewModel.InvokeUpdateTime += UpdateTime;
        }

        private void UpdateTime(ClockDto dto)
        {
            _clockText.text = dto.ClockText;
            _clock.Set(dto);
        }

        private void OnDestroy()
        {
            _backButton.onClick.RemoveListener(_viewModel.InvokeClose);
            _viewModel.InvokeUpdateTime -= UpdateTime;
        }
    }
}