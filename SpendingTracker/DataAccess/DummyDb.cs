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
                CarriedOverMoneyFromPreviousDay = 0,
                DailyBudget = DayManager.DAY_INITIAL_BUDGET,
                OriginalAvailableMoney = DayManager.DAY_INITIAL_BUDGET,
                AvailableMoney = DayManager.DAY_INITIAL_BUDGET,
                IsBudgetExceeded= false,

                Transactions = new ObservableCollection<Transaction>()
                {
                    
                }
            }
            };
        }
    }
}

