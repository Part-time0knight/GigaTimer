using App.Presentation.ViewModel;
using Core.Data.Dto;
using System.Collections.Generic;

namespace App.Domain.Dto
{
    public class AlarmDto : IDto
    {
        public AlarmSetViewModel.Format Format;

        public readonly Dictionary<AlarmSetViewModel.Hand, float> Hands;

        public string HourText;

        public string MinuteText;

        public string SecondText;

        public AlarmDto() 
        {
            Hands = new();
        }
    }
}