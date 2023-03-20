using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SpendingTracker.Managers;

namespace SpendingTracker.Models;

public class Day 
{
    public ObservableCollection<Transaction> AllTransactions { get; set; } = new ObservableCollection<Transaction>();
    
    //[ObservableProperty] //TODO: not sure if it's okay to implement ObservableObject in Model class. 
    public bool IsBudgetExceeded { get; set; } = false;
    
    //[ObservableProperty]
    public decimal Budget { get; set; } = DayManager.DAY_INITIAL_BUDGET;
}