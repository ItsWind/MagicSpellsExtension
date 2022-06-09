using System;
using System.Collections.Generic;
using System.IO;
using TaleWorlds.InputSystem;

namespace MagicSpells
{
    public class Config
    {
        private string configFilePath = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.Length - 26) + "Modules\\MagicSpells\\config.txt";

        private Dictionary<string, double> configValues = new();
        private readonly string configFileString =
            "Set the base heal amount. Default is 10\n" +
            "baseHealAmount=10.0\n\n" +

            "Displays all healing amounts. Mainly for debugging purposes. Default is 0. Set to 1 to enable.\n" +
            "debugDisplayHealAmounts=0.0\n\n";

        private void CreateConfigFile()
        {
            StreamWriter sw = new(this.configFilePath);
            sw.WriteLine(this.configFileString);
            sw.Close();
        }

        public void LoadConfig()
        {
            StreamReader sr = new(this.configFilePath);
            string line;
            // Read and display lines from the file until the end of
            // the file is reached.
            while ((line = sr.ReadLine()) != null)
            {
                int indexOfEqualSign = line.IndexOf('=');
                if (indexOfEqualSign != -1)
                {
                    string key = line.Substring(0, indexOfEqualSign);
                    string value = line.Substring(indexOfEqualSign + 1);
                    configValues[key] = Convert.ToDouble(value);
                }
            }
            sr.Close();
        }

        public Config()
        {
            if (!File.Exists(this.configFilePath))
                CreateConfigFile();
            LoadConfig();
        }

        public double GetKeyValue(string key)
        {
            return configValues[key];
        }
    }
}
