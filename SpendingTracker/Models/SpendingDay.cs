using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SpendingTracker.Models;

public partial class SpendingDay : ObservableObject
{
    public ObservableCollection<SpendingTransaction> AllTransactions { get; set; } = new ObservableCollection<SpendingTransaction>();
    
    [ObservableProperty] //TODO: not sure if it's okay to implement ObservableObject in Model class. 
    private bool isBudgetExceeded = false;
    
    [ObservableProperty]
    private decimal budget = 0;
}