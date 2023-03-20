using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SpendingTracker.Managers;

namespace SpendingTracker.Models;

public class Day 
{
    /// <summary>
    /// Unique Identifier for the Day
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The amount of money added to the available budget amount
    /// </summary>
    public decimal DailyBudget { get; set; } 

    public bool IsBudgetExceeded { get; set; }

    /// <summary>
    /// The individual spending amounts to the day
    /// </summary>
    public ObservableCollection<Transaction> Transactions { get; set; }
}