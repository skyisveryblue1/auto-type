using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace auto_type
{
    // Class to store button configuration data, marked as serializable for binary serialization
    [Serializable]
    public class ButtonInfo
    {
        // Properties for button attributes
        public string Name { get; set; }
        public string Group { get; set; }
        public string Hint { get; set; }
        public string TextToType { get; set; }

        // Property for button index
        public int Index { get; set; }

        // Properties for button appearance
        public Font Font { get; set; }
        public Color TextColor { get; set; }
        public Color BackColor { get; set; }
    }

    // Class to manage data persistence for buttons and groups, marked as serializable
    [Serializable]
    public class Data
    {
        // Constant for the data file name
        private const string dataFile = "buttons.dat";

        // Static properties to store lists of buttons and groups
        public static List<ButtonInfo> Buttons { get; set; }
        public static List<string> Groups { get; set; }

        // Method to load button and group data from file
        public static void LoadData()
        {
            // Attempt to load data, handling potential exceptions
            try
            {
                // Check if the data file exists
                if (File.Exists(dataFile))
                {
                    // Open the file stream for reading
                    using (var stream = File.Open(dataFile, FileMode.Open))
                    {
                        // Create a binary formatter for deserialization
                        var formatter = new BinaryFormatter();
                        // Deserialize groups and buttons from the file
                        Groups = formatter.Deserialize(stream) as List<string>;
                        Buttons = formatter.Deserialize(stream) as List<ButtonInfo>;
                    }
                }
                else
                {
                    // Initialize empty lists if file doesn't exist
                    if (Groups == null) Groups = new List<string>();
                    if (Buttons == null) Buttons = new List<ButtonInfo>();

                    // Add default group if it doesn't exist
                    if (!Groups.Contains("Default"))
                        Groups.Add("Default");
                }
            }
            // Catch and handle any exceptions during loading
            catch (Exception ex)
            {
                // Display error message if loading fails
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }

        // Method to save button and group data to file
        public static void SaveData()
        {
            // Attempt to save data, handling potential exceptions
            try
            {
                // Open the file stream for writing
                using (var stream = File.Open(dataFile, FileMode.Create))
                {
                    // Create a binary formatter for serialization
                    var formatter = new BinaryFormatter();
                    // Serialize groups and buttons to the file
                    formatter.Serialize(stream, Groups);
                    formatter.Serialize(stream, Buttons);
                }
            }
            // Catch and handle any exceptions during saving
            catch (Exception ex)
            {
                // Display error message if saving fails
                MessageBox.Show($"Error saving data: {ex.Message}");
            }
        }
    }
}
