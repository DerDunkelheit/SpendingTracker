using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using SpendingTracker.Managers;
using SpendingTracker.Models;

namespace SpendingTracker.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private string spendingTextBoxText = string.Empty;
    [ObservableProperty]
    private SpendingDay currentDay = new SpendingDay();
    [ObservableProperty]
    private float currentBudget = 0;
    
    public ObservableCollection<SpendingDay> TotalDays { get; set; } = new ObservableCollection<SpendingDay>();
    
    private readonly string SAVE_FILE_NAME = "save.json";


    public MainWindowViewModel()
    {
        if (!File.Exists(SAVE_FILE_NAME))
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
            UpdateCurrentBudget(spendingInput);
        }
        else
        {
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Assertion", "Invalid or Empty Input, only numbers are allowed");
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

    private void UpdateCurrentBudget(float valueToAdd)
    {
        CurrentDay.Budget += valueToAdd;
        CurrentDay.IsBudgetExceeded = CurrentDay.Budget < 0;

        UpdateCurrentBudget();

        Save();
    }

    private void IncreaseDayNumber()
    {
        TotalDays.Add(new SpendingDay { Budget = DayManager.DAY_INITIAL_BUDGET });
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
        string json = JsonConvert.SerializeObject(TotalDays);
        File.WriteAllText(SAVE_FILE_NAME, json);
    }
    
    private void Load()
    {
        string json = File.ReadAllText(SAVE_FILE_NAME);
        TotalDays = JsonConvert.DeserializeObject<ObservableCollection<SpendingDay>>(json);
        CurrentDay = TotalDays.Last();

        UpdateCurrentBudget();
    }
    
    #endregion
}