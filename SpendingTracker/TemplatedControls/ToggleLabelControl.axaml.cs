using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace SpendingTracker.TemplatedControls;

public class ToggleLabelControl : TemplatedControl
{
    public static readonly StyledProperty<string> LabelTextProperty = AvaloniaProperty.Register<ToggleLabelControl, string>(
        nameof(LabelText), "Default Text");

    public static readonly StyledProperty<string> LabelSecondTextProperty = AvaloniaProperty.Register<ToggleLabelControl, string>(
        nameof(LabelSecondText), "0");

    public static readonly StyledProperty<bool> IsElementCheckedProperty = AvaloniaProperty.Register<ToggleLabelControl, bool>(
        nameof(IsElementChecked));

    
    public string LabelText
    {
        get => GetValue(LabelTextProperty);
        set => SetValue(LabelTextProperty, value);
    }
    
    public string LabelSecondText
    {
        get => GetValue(LabelSecondTextProperty);
        set => SetValue(LabelSecondTextProperty, value);
    }
    
    public bool IsElementChecked
    {
        get => GetValue(IsElementCheckedProperty);
        set => SetValue(IsElementCheckedProperty, value);
    }
}