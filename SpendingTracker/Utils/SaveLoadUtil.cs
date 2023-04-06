using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;

namespace SpendingTracker.Utils;

public class SaveLoadUtil
{
    public static string FULL_FILE_PATH = "save.json";

    /// <summary>
    /// Gets the text file information from the FullFilePath and 
    /// returns an ObservableCollection of T
    /// </summary>
    /// <typeparam name="T">The Type to Convert the Text file to</typeparam>
    /// <returns>An ObservableCollection<typeparamref name="T"/></returns>
    public static ObservableCollection<T> GetCollectionFromJsonFile<T>()
    {
        var result = new ObservableCollection<T>();
        
        if (File.Exists(FULL_FILE_PATH))
        {
            string json = File.ReadAllText(FULL_FILE_PATH);
            
            if (json.Length > 0)
            {
                result = JsonConvert.DeserializeObject<ObservableCollection<T>>(json);
            }
        }
        else
        {
            File.Create(FULL_FILE_PATH);
        }

        return result;
    }

    /// <summary>
    /// Saves a passed in ObservableCollection of T to a Text File
    /// </summary>
    /// <typeparam name="T">The Type to Convert the Text file to</typeparam>
    /// <param name="collection">Passed in ObservableCollection of T</param>
    public static void SaveCollectionToJsonFile<T>(ObservableCollection<T> collection)
    {
        string json = JsonConvert.SerializeObject(collection);
        File.WriteAllText(FULL_FILE_PATH, json);
    }
}