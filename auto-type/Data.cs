using System;
using System.Collections.Generic;
using System.Drawing;
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
        public string Group { get; set; }
        public string Hint { get; set; }
        public string TextToType { get; set; }
        
        public int Index { get; set; }

        public Font Font { get; set; }
        public Color TextColor { get; set; }
        public Color BackColor { get; set; }
    }

    [Serializable]
    public class Data
    {
        private const string dataFile = "buttons.dat";

        public static List<ButtonInfo> Buttons { get; set; }
        public static List<string> Groups { get; set; }

        public static void LoadData()
        {
            try
            {
                if (File.Exists(dataFile))
                {
                    using (var stream = File.Open(dataFile, FileMode.Open))
                    {
                        var formatter = new BinaryFormatter();
                        Groups = formatter.Deserialize(stream) as List<string>;
                        Buttons = formatter.Deserialize(stream) as List<ButtonInfo>;
                    }
                }
                else
                {
                    if (Groups == null) Groups = new List<string>();
                    if (Buttons == null) Buttons = new List<ButtonInfo>();

                    if (!Groups.Contains("Default"))
                        Groups.Add("Default");
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
                    formatter.Serialize(stream, Groups);
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
