using CommunityToolkit.Mvvm.ComponentModel;

namespace SpendingTracker.Models;

public partial class SpendingDay : ObservableObject
{
    [ObservableProperty] //TODO: not sure if it's okay to implement ObservableObject in Model class. 
    private bool isBudgetExceeded = false;
    
    [ObservableProperty]
    private float budget = 0;
}