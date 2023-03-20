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
    /// <summary>
    /// The text in the Money Spent textbox
    /// </summary>
    [ObservableProperty]
    private string spendingTextBoxText = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [ObservableProperty]
    private Day currentDay;
    [ObservableProperty]
    private decimal currentBudget = 0;

    private ObservableCollection<Day> days = new ObservableCollection<Day>();

    public ObservableCollection<Day> Days
    {
        get => days;
        set => SetProperty(ref days, value);
    }





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

private void RemoveLastSpendingCommand()
{
if (CurrentDay.Transactions.Count > 0)
{
var lastTransaction = CurrentDay.Transactions.Last();
decimal lastTransactionSpent = lastTransaction.Amount;
decimal valueToRestore = lastTransactionSpent * -1;

CurrentDay.Transactions.Remove(lastTransaction);
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
if (Days.Count > 1)
{
Days.RemoveAt(Days.Count - 1);
CurrentDay = Days.Last();

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

private void AddSpendingTransaction(decimal valueToAdd)
{
CurrentDay.Transactions.Add(new Transaction { Amount = valueToAdd });
UpdateCurrentDayBudget(valueToAdd);

UpdateCurrentBudget();

Save();
}

private void UpdateCurrentDayBudget(decimal valueToAdd)
{
CurrentDay.DailyBudget += valueToAdd;
CurrentDay.IsBudgetExceeded = CurrentDay.DailyBudget < 0;
}

private void IncreaseDayNumber()
{
Day newDay = new Day { DailyBudget = DayManager.DAY_INITIAL_BUDGET };

Days.Add(newDay);
CurrentDay = Days.Last();

UpdateCurrentBudget();

Save();
}

private void UpdateCurrentBudget()
{
//TODO: come up with improvements with MVVM design to remove this method.
CurrentBudget = Days.Sum(day => day.DailyBudget);
}

#region Save/Load

private void Save()
{
string json = JsonConvert.SerializeObject(Days);
File.WriteAllText(SAVE_FILE_NAME, json);
}

private void Load()
{
string json = File.ReadAllText(SAVE_FILE_NAME);
Days = JsonConvert.DeserializeObject<ObservableCollection<Day>>(json);
CurrentDay = Days.Last();

UpdateCurrentBudget();
}
    
    #endregion
}