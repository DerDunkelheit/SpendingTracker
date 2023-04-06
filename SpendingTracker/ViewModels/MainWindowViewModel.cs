using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using SpendingTracker.Interfaces;
using SpendingTracker.Managers;
using SpendingTracker.Models;
using SpendingTracker.Utils;

namespace SpendingTracker.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private string spendingTextBoxText = string.Empty;
    [ObservableProperty]
    private SpendingDay currentDay = new SpendingDay();
    [ObservableProperty]
    private float currentBudget = 0;
    [ObservableProperty] 
    private float dayIncreaseValue = DayManager.DAY_INITIAL_BUDGET;
    
    public ObservableCollection<SpendingDay> TotalDays { get; set; } = new ObservableCollection<SpendingDay>();

    public IView View { get; set; }


    public MainWindowViewModel()
    {
        if (!File.Exists(SaveLoadUtil.FULL_FILE_PATH))
        {
            IncreaseDayNumber();
        }
        else
        {
            Load();
        }
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
        if (CurrentDay.AllTransactions.Count > 0)
        {
            var lastTransaction = CurrentDay.AllTransactions.Last();
            float lastTransactionSpent = lastTransaction.SpentValue;
            float valueToRestore = lastTransactionSpent * -1;

            CurrentDay.AllTransactions.Remove(lastTransaction);
            UpdateCurrentDayBudget(valueToRestore);
            UpdateCurrentBudget();
            
            Save();
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
            CurrentDay = TotalDays.Last();

            UpdateCurrentBudget();

            Save();
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
        CurrentDay.AllTransactions.Add(new SpendingTransaction { SpentValue = valueToAdd });
        UpdateCurrentDayBudget(valueToAdd);

        UpdateCurrentBudget();

        if (View != null)
        {
            View.ClearTextBox();
        }

        Save();
    }

    private void UpdateCurrentDayBudget(float valueToAdd)
    {
        CurrentDay.Budget += valueToAdd;
        CurrentDay.IsBudgetExceeded = CurrentDay.Budget < 0;
    }

    private void IncreaseDayNumber()
    {
        SpendingDay newDay = new SpendingDay { Budget = DayManager.DAY_INITIAL_BUDGET, Date = DateTime.Today};

        TotalDays.Add(newDay);
        CurrentDay = TotalDays.Last();

        UpdateCurrentBudget();

        Save();
    }

    private void UpdateCurrentBudget()
    {
        //TODO: come up with improvements with MVVM design to remove this method.
        CurrentBudget = TotalDays.Sum(day => day.Budget);
    }

    #region Save/Load
    
    private void Save()
    {
        SaveLoadUtil.SaveCollectionToJsonFile(TotalDays);
    }
    
    private void Load()
    {
        TotalDays = SaveLoadUtil.GetCollectionFromJsonFile<SpendingDay>();
        CurrentDay = TotalDays.Last();

        UpdateCurrentBudget();
    }
    
    #endregion
}