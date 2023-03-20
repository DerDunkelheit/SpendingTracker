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

    private ObservableCollection<Day> totalDays = new ObservableCollection<Day>();

    public ObservableCollection<Day> TotalDays
    {
        get => totalDays;
        set => SetProperty(ref totalDays, value);
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
if (CurrentDay.AllTransactions.Count > 0)
{
var lastTransaction = CurrentDay.AllTransactions.Last();
decimal lastTransactionSpent = lastTransaction.SpentValue;
decimal valueToRestore = lastTransactionSpent * -1;

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

private void AddSpendingTransaction(decimal valueToAdd)
{
CurrentDay.AllTransactions.Add(new Transaction { SpentValue = valueToAdd });
UpdateCurrentDayBudget(valueToAdd);

UpdateCurrentBudget();

Save();
}

private void UpdateCurrentDayBudget(decimal valueToAdd)
{
CurrentDay.Budget += valueToAdd;
CurrentDay.IsBudgetExceeded = CurrentDay.Budget < 0;
}

private void IncreaseDayNumber()
{
Day newDay = new Day { Budget = DayManager.DAY_INITIAL_BUDGET };

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
string json = JsonConvert.SerializeObject(TotalDays);
File.WriteAllText(SAVE_FILE_NAME, json);
}

private void Load()
{
string json = File.ReadAllText(SAVE_FILE_NAME);
TotalDays = JsonConvert.DeserializeObject<ObservableCollection<Day>>(json);
CurrentDay = TotalDays.Last();

UpdateCurrentBudget();
}
    
    #endregion
}