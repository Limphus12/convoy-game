using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace com.limphus.save_system
{
    public static class SaveManager
    {
        public static readonly string SAVE_FOLDER = Application.persistentDataPath + "/GameData/";
        public static readonly string SAVE_FILE = "/save.json", SETTINGS_SAVE_FILE = "/settings.json", CONVOY_SAVE_FILE = "/convoy.json";

        public static void Init()
        {
            //Tests if the save folder exists
            if (!Directory.Exists(SAVE_FOLDER))
            {
                //creates the save folder and assigns the directory to the SAVE_FOLDER string.
                Directory.CreateDirectory(SAVE_FOLDER);

                //Initializing files
                InitFiles();
            }

            //If we do have the save folder
            else if (Directory.Exists(SAVE_FOLDER))
            {
                //Try and load files
                LoadFiles();
            }
        }

        private static void InitFiles()
        {
            //initialising settings save file
            if (!File.Exists(SAVE_FOLDER + SETTINGS_SAVE_FILE))
            {
                SaveSystem.InitSettings();
            }

            //initialising convoy save file
            if (!File.Exists(SAVE_FOLDER + CONVOY_SAVE_FILE))
            {
                SaveSystem.InitConvoy();
            }
        }

        private static void LoadFiles()
        {
            //loading the settings save file
            if (File.Exists(SAVE_FOLDER + SETTINGS_SAVE_FILE))
            {
                SaveSystem.LoadSettings();
            }

            //loading the convoy save file
            if (File.Exists(SAVE_FOLDER + CONVOY_SAVE_FILE))
            {
                SaveSystem.LoadConvoy();
            }
        }

        //save function
        public static void Save(string saveFile, string saveString)
        {
            File.WriteAllText(SAVE_FOLDER + saveFile, saveString);
        }

        //load function
        public static string Load(string saveFile)
        {
            //checking if the file exists.
            if (File.Exists(SAVE_FOLDER + saveFile))
            {
                //grabbing the data from the .txt file
                string saveString = File.ReadAllText(SAVE_FOLDER + saveFile);

                //returns the save string
                return saveString;
            }

            //if we cannot find the file, return null
            else return null;
        }
    }
}