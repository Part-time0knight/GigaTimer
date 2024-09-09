using Core.Data.Dto;

namespace App.Domain.Dto
{
    public class ClockDto : IDto
    {
        public string ClockText;
        public float SecondHandAngle;
        public float MinuteHandAngle;
        public float HourHandAngle;
    }
}