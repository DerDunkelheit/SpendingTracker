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

        // If we already have a dailyBudget value from today...
        if (dailyBudget != null)
        {
        // Set the Original Available money for the day from the carried over plus the daily budget
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

        // If we already have a Carried over money from the the previous day...
        if (carriedOverMoneyFromPreviousDay != null)
        {
        // Set the Original Available money for the day from the carried over plus the daily budget
        OriginalAvailableMoney = carriedOverMoneyFromPreviousDay + dailyBudget;
        }
        }
    }

    /// <summary>
    /// The Day's original available money to spend
    /// </summary>
    private decimal originalAvailableMoney;
    public decimal OriginalAvailableMoney
    {
        get => originalAvailableMoney;
        set
        {
        originalAvailableMoney = value;

        // If the OriginalAvailableMoney is set (which happens only once)
        // Set the AvailableMoney for the day without any Transaction Deductions
        AvailableMoney = value;
        }
    }

    /// <summary>
    /// The Running total of available Money for the day including:
    ///      The carried over money from the previous day plus
    ///      The Daily Budget added on minus
    ///      The Spending transactions for the day      
    /// </summary>
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

        // If we are setting transactions and there are more than one,
        // update the Available Money for the day with the new spending transactions
        if (transactions.Count > 0)
        {
        AvailableMoney = originalAvailableMoney - transactions.Sum(x => x.Amount);
        }

        // If there are no transactions then just set the AvailableMoney
        // to the OriginalAvailableMoney
        else AvailableMoney = originalAvailableMoney;
        }
    }
}