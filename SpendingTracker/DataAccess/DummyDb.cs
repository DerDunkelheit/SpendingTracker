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
        /// <summary>
        /// Method to return a collection of Days with one day and no transactions
        /// </summary>
        /// <returns>Collection of one day with just the settings for a first day</returns>
        public static ObservableCollection<Day> GetFirstDayCollectionFromDummyDb()
        {

        // Return a new collection of days with one day and just the settings for a first day
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

