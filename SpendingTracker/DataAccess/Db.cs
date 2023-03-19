using Newtonsoft.Json;
using SpendingTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SpendingTracker.DataAccess
{
    public static class Db
    {

        private static string path;

        private static string filename;

        /// <summary>
        /// The FullFilePath to Get and Save Collection information
        /// </summary>
        /// 
        public static string FullFilePath = "save.txt";

        //public static string FullFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Assets\DaysTotals.txt");

        #region Save/Load

        /// <summary>
        /// Gets the text file information from the FullFilePath and 
        /// returns an ObservableCollection of T
        /// </summary>
        /// <typeparam name="T">The Type to Convert the Text file to</typeparam>
        /// <returns>An ObservableCollection<typeparamref name="T"/></returns>
        public static ObservableCollection<T> GetCollectionFromJsonFile<T>()
        {
        // Create a new result property to be returned
        var result = new ObservableCollection<T>();

        // If the File exists...
        if (File.Exists(FullFilePath))
        {
        // String representing the text from the file
        string json = File.ReadAllText(FullFilePath);

        // If the File exists and has some characters inside the file
        //     then Deserialize the object... otherwise the result remains 
        //     as a new Collection
        if (json.Length > 0)

            // Convert the String to an ObservableCollection of T
            result = JsonConvert.DeserializeObject<ObservableCollection<T>>(json);
        }

        // If the file doesn't exist create a new blank file
        else File.Create(FullFilePath);

        // Return the created ObservableCollection of T
        return result;
        }

        /// <summary>
        /// Saves a passed in ObservableCollection of T to a Text File
        /// </summary>
        /// <typeparam name="T">The Type to Convert the Text file to</typeparam>
        /// <param name="collection">Passed in ObservableCollection of T</param>
        public static void SaveCollectionToJsonFile<T>(ObservableCollection<T> collection)
        {
        // Convert the ObservableCollection into a Json String
        string json = JsonConvert.SerializeObject(collection);

        // Write the Converted String to a Text File
        File.WriteAllText(FullFilePath, json);
        }

        #endregion


    }
}
