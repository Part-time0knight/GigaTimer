using App.Domain.Dto;
using App.Model.Clock.Alarm;
using App.Presentation.View;
using App.Presentation.ViewModel;
using Core.MVVM.ViewModel;
using Core.MVVM.Windows;
using System;


public class AlarmFinishViewModel : AbstractViewModel
{
    public event Action<ClockDto> InvokeUpdateTime;

    private readonly AlarmService _alarm;
    private readonly ClockDto _dto;

    protected override Type Window => typeof(AlarmFinishView);

    public AlarmFinishViewModel(IWindowFsm windowFsm, AlarmService alarm) : base(windowFsm)
    {
        _alarm = alarm;
        _dto = new();
    }



    public override void InvokeClose()
    {
        _windowFsm.CloseWindow();
    }

    public override void InvokeOpen()
    {
        _windowFsm.OpenWindow(Window, inHistory: true);
    }

    protected override void HandleOpenedWindow(Type uiWindow)
    {
        base.HandleOpenedWindow(uiWindow);
        if (uiWindow != Window)
            return;
        Update();
    }

    private void Update()
    {
        ClockConverter.MakeConvert(_dto, _alarm.FinishTime);
        InvokeUpdateTime?.Invoke(_dto);
    }
}
