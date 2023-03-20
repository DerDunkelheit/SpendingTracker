using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using SpendingTracker.DataAccess;
using SpendingTracker.Managers;
using SpendingTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// The Text shown in the Textbox to enter spending amount
        /// </summary>
        [ObservableProperty]
        private string spendingTextBoxText = string.Empty;

        /// <summary>
        /// The currently Selected Day to show Transactions and available budget for
        /// </summary>
        [ObservableProperty]
        private Day selectedDay;


        [ObservableProperty]
        private decimal currentBudget = 0;

        private ObservableCollection<Day> days = new ObservableCollection<Day>();

        public ObservableCollection<Day> Days
        {
            get => days;
            set => SetProperty(ref days, value);
        }

        public MainViewModel()
        {
        var td = Db.GetCollectionFromJsonFile<Day>();

        if (td.Count == 0)
        {
        AddNewDay();
        }

       

        }

        [RelayCommand]
        private void AddSpending()
        {
        if (decimal.TryParse(SpendingTextBoxText, out decimal spendingInput))
        {
        AddSpendingTransaction(spendingInput);
        }
        else
        {
        var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Assertion", "Invalid or Empty Input, only numbers are allowed");
        messageBoxStandardWindow.Show();
        }
        }

        [RelayCommand]
        private void RemoveLastSpending()
        {
        if (SelectedDay.AllTransactions.Count > 0)
        {
        var lastTransaction = SelectedDay.AllTransactions.Last();
        decimal lastTransactionSpent = lastTransaction.SpentValue;
        decimal valueToRestore = lastTransactionSpent * -1;

        SelectedDay.AllTransactions.Remove(lastTransaction);
        UpdateCurrentDayBudget(valueToRestore);
        UpdateCurrentBudget();

        Db.SaveCollectionToJsonFile<Day>(Days);
        }
        else
        {
        var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Assertion", "You haven't made any spending yet");
        messageBoxStandardWindow.Show();
        }
        }

        [RelayCommand]
        private void AddNewDay()
        {
        IncreaseDayNumber();
        }

        [RelayCommand]
        private void RemoveLastDay()
        {
        if (Days.Count > 1)
        {
        Days.RemoveAt(Days.Count - 1);
        SelectedDay = Days.Last();

        UpdateCurrentBudget();

        Db.SaveCollectionToJsonFile<Day>(Days);
        }
        else
        {
        var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxStandardWindow("Assertion", "Cannot remove the first day");
        messageBoxStandardWindow.Show();
        }
        }

        private void AddSpendingTransaction(decimal valueToAdd)
        {
        SelectedDay.AllTransactions.Add(new Transaction { SpentValue = valueToAdd });
        UpdateCurrentDayBudget(valueToAdd);

        UpdateCurrentBudget();

        Db.SaveCollectionToJsonFile<Day>(Days);
        }

       
        private void UpdateCurrentDayBudget(decimal valueToAdd)
        {
        SelectedDay.Budget += valueToAdd;
        SelectedDay.IsBudgetExceeded = SelectedDay.Budget < 0;
        }

        private void IncreaseDayNumber()
        {
        Day newDay = new Day { Budget = DayManager.DAY_INITIAL_BUDGET };

        Days.Add(newDay);
        SelectedDay = Days.Last();

        UpdateCurrentBudget();

        Db.SaveCollectionToJsonFile<Day>(Days);
        }

        private void UpdateCurrentBudget()
        {
        //TODO: come up with improvements with MVVM design to remove this method.
        SelectedDay.Budget = Days.Sum(day => day.Budget);
        }

       
    }
}
