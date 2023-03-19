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
        [ObservableProperty]
        private string spendingTextBoxText = string.Empty;
        [ObservableProperty]
        private SpendingDay selectedDay;
        [ObservableProperty]
        private decimal currentBudget = 0;

        private ObservableCollection<SpendingDay> totalDays = new ObservableCollection<SpendingDay>();

        public ObservableCollection<SpendingDay> TotalDays
        {
            get => totalDays;
            set => SetProperty(ref totalDays, value);
        }

        public MainViewModel()
        {
        var td = Db.GetCollectionFromJsonFile<SpendingDay>();

        if (td.Count == 0)
        {
        AddNewDay();
        }

        TotalDays = td;

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

        Db.SaveCollectionToJsonFile<SpendingDay>(TotalDays);
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
        if (TotalDays.Count > 1)
        {
        TotalDays.RemoveAt(TotalDays.Count - 1);
        SelectedDay = TotalDays.Last();

        UpdateCurrentBudget();

        Db.SaveCollectionToJsonFile<SpendingDay>(TotalDays);
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
        SelectedDay.AllTransactions.Add(new SpendingTransaction { SpentValue = valueToAdd });
        UpdateCurrentDayBudget(valueToAdd);

        UpdateCurrentBudget();

        Db.SaveCollectionToJsonFile<SpendingDay>(TotalDays);
        }

       
        private void UpdateCurrentDayBudget(decimal valueToAdd)
        {
        SelectedDay.Budget += valueToAdd;
        SelectedDay.IsBudgetExceeded = SelectedDay.Budget < 0;
        }

        private void IncreaseDayNumber()
        {
        SpendingDay newDay = new SpendingDay { Budget = DayManager.DAY_INITIAL_BUDGET };

        TotalDays.Add(newDay);
        SelectedDay = TotalDays.Last();

        UpdateCurrentBudget();

        Db.SaveCollectionToJsonFile<SpendingDay>(TotalDays);
        }

        private void UpdateCurrentBudget()
        {
        //TODO: come up with improvements with MVVM design to remove this method.
        CurrentBudget = TotalDays.Sum(day => day.Budget);
        }

       
    }
}
