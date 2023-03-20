using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpendingTracker.Managers;
using SpendingTracker.Models;

namespace SpendingTracker.DataAccess
{
    public static class DummyDb
    {
public static ObservableCollection<Day> GetCollectionFromDummyDb()
        {
        return new ObservableCollection<Day>()
        {
            new Day()
            {
                Id= 1,
                DailyBudget = DayManager.DAY_INITIAL_BUDGET - 25,
                IsBudgetExceeded= false,
                Transactions = new ObservableCollection<Transaction>() { new Transaction() { Amount = 25} } }

            };
        }
        }
    }

