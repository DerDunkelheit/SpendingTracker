using CommunityToolkit.Mvvm.ComponentModel;
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
        private float currentBudget = 0;

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
        AddNewDayCommand();
        }

        TotalDays = td;

        }

        private void AddSpendingCommand()
        {
        if (float.TryParse(SpendingTextBoxText, out float spendingInput))
        {
        AddSpendingTransaction(spendingInput);
        }
        else
        {
        var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Assertion", "Invalid or Empty Input, only numbers are allowed");
        messageBoxStandardWindow.Show();
        }
        }

        private void RemoveLastSpendingCommand()
        {
        if (SelectedDay.AllTransactions.Count > 0)
        {
        var lastTransaction = SelectedDay.AllTransactions.Last();
        float lastTransactionSpent = lastTransaction.SpentValue;
        float valueToRestore = lastTransactionSpent * -1;

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

        private void AddNewDayCommand()
        {
        IncreaseDayNumber();
        }

        private void RemoveLastDayCommand()
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

        private void AddSpendingTransaction(float valueToAdd)
        {
        SelectedDay.AllTransactions.Add(new SpendingTransaction { SpentValue = valueToAdd });
        UpdateCurrentDayBudget(valueToAdd);

        UpdateCurrentBudget();

        Db.SaveCollectionToJsonFile<SpendingDay>(TotalDays);
        }

        private void UpdateCurrentDayBudget(float valueToAdd)
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
