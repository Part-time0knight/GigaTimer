using App.Domain.Dto;
using System;

namespace App.Presentation.ViewModel
{
    public static class ClockConverter
    {
        public static void MakeConvert(ClockDto dto, DateTime time)
        {
            if (time.Hour < 10)
                dto.ClockText = "0" + time.ToLongTimeString();
            else
                dto.ClockText = time.ToLongTimeString();

            dto.SecondHandAngle = -6f * time.Second; //-360 * time.Second/60
            dto.MinuteHandAngle = -6f * time.Minute; //-360 * time.Minute/60
            dto.HourHandAngle = -30f * (time.Hour % 12); //-360f * (time.Hour % 12)/12
        }
    }
}