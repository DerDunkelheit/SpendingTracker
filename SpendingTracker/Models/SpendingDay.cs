using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SpendingTracker.Models;

public partial class SpendingDay : ObservableObject
{
    public ObservableCollection<SpendingTransaction> AllTransactions { get; set; } = new ObservableCollection<SpendingTransaction>();
    
    [ObservableProperty]
    private bool isBudgetExceeded = false;
    
    [ObservableProperty]
    private float budget = 0;
    
    public DateTime Date { get; set; }
}