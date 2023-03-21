using System.Collections.ObjectModel;
using System.Linq;
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
    /// The amount of money that has been carried over from the previous 
    /// day's carried over money plus the daily budget minus the spending
    /// transactions
    /// </summary>
    private decimal carriedOverMoneyFromPreviousDay;
    public decimal CarriedOverMoneyFromPreviousDay
    {
        get => carriedOverMoneyFromPreviousDay;
        set
        {
        carriedOverMoneyFromPreviousDay = value;
        if (dailyBudget != null)
        {
        OriginalAvailableMoney = carriedOverMoneyFromPreviousDay + dailyBudget;

        }
        }
    }

    /// <summary>
    /// The amount of money added to the available budget amount
    /// </summary>
    private decimal dailyBudget;
    public decimal DailyBudget
    {
        get => dailyBudget;
        set
        {
        dailyBudget = value;
        if (carriedOverMoneyFromPreviousDay != null)
        {
        OriginalAvailableMoney = carriedOverMoneyFromPreviousDay + dailyBudget;

        }
        }
    }
    private decimal originalAvailableMoney;

    public decimal OriginalAvailableMoney 
    { 
        get => originalAvailableMoney;
        set
        {
        originalAvailableMoney = value;
        AvailableMoney = value ;
        }
    }

    public decimal AvailableMoney { get; set; }

    /// <summary>
    /// Flag indicating if the spending has exceeded the available money
    /// </summary>
    public bool IsBudgetExceeded { get; set; }

    /// <summary>
    /// The individual spending amounts for each day
    /// </summary>
    private ObservableCollection<Transaction> transactions;
    public ObservableCollection<Transaction> Transactions
    {
        get => transactions;
        set
        {
        transactions = value;
        if (transactions.Count > 0)
        {
        AvailableMoney = originalAvailableMoney - transactions.Sum(x => x.Amount);

        }
        else AvailableMoney = originalAvailableMoney;
        }
    }
}