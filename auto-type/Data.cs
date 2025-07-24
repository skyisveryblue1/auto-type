using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace auto_type
{
    [Serializable]
    public class ButtonInfo
    {
        public string Name { get; set; }
        public string TextToType { get; set; }
        public string Category { get; set; }
        public int Index { get; set; }
    }

    [Serializable]
    public class Data
    {
        private const string dataFile = "buttons.dat";

        public static List<ButtonInfo> Buttons { get; set; }
        public static List<string> Categories { get; set; }

        public static void LoadData()
        {
            try
            {
                if (File.Exists(dataFile))
                {
                    using (var stream = File.Open(dataFile, FileMode.Open))
                    {
                        var formatter = new BinaryFormatter();
                        Categories = formatter.Deserialize(stream) as List<string>;
                        if (!Categories.Contains("Default"))
                            Categories.Add("Default");
                        Buttons = formatter.Deserialize(stream) as List<ButtonInfo>;
                    }
                }
                else
                {
                    if (Categories == null) Categories = new List<string>();
                    if (Buttons == null) Buttons = new List<ButtonInfo>();

                    if (!Categories.Contains("Default"))
                        Categories.Add("Default");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }
        public static void SaveData()
        {
            try
            {
                using (var stream = File.Open(dataFile, FileMode.Create))
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(stream, Categories);
                    formatter.Serialize(stream, Buttons);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}");
            }
        }
    }
}
