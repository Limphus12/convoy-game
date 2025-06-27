using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using com.limphus.settings;
using com.limphus.utilities;
using com.limphus.convoy;

namespace com.limphus.save_system
{
    public class SaveSystem : MonoBehaviour
    {
        public static SaveSystem instance;

        #region Events

        public static event EventHandler<SaveSystemEvents.OnSettingsChangedEventArgs> OnSettingsLoadedEvent, OnSettingsChangedEvent;
        protected static void OnSettingsLoaded(SettingsData data) => OnSettingsLoadedEvent?.Invoke(typeof(SaveSystem), new SaveSystemEvents.OnSettingsChangedEventArgs { i = data });
        protected static void OnSettingsChanged(SettingsData data) => OnSettingsChangedEvent?.Invoke(typeof(SaveSystem), new SaveSystemEvents.OnSettingsChangedEventArgs { i = data });


        public static event EventHandler<SaveSystemEvents.OnConvoyChangedEventArgs> OnConvoyLoadedEvent, OnConvoyChangedEvent;
        protected static void OnConvoyLoaded(ConvoyData data) => OnConvoyLoadedEvent?.Invoke(typeof(SaveSystem), new SaveSystemEvents.OnConvoyChangedEventArgs { i = data });
        protected static void OnConvoyChanged(ConvoyData data) => OnConvoyChangedEvent?.Invoke(typeof(SaveSystem), new SaveSystemEvents.OnConvoyChangedEventArgs { i = data });


        public static event EventHandler<SaveSystemEvents.OnGameChangedEventArgs> OnGameLoadedEvent, OnGameChangedEvent;
        protected static void OnGameLoaded(GameData data) => OnGameLoadedEvent?.Invoke(typeof(SaveSystem), new SaveSystemEvents.OnGameChangedEventArgs { i = data });
        protected static void OnGameChanged(GameData data) => OnGameChangedEvent?.Invoke(typeof(SaveSystem), new SaveSystemEvents.OnGameChangedEventArgs { i = data });

        #endregion

        #region Init

        private void Awake()
        {
            if (!instance) instance = this;
            else if (instance) Destroy(this);

            SaveManager.Init();

            PauseManager.OnPausedChangedEvent += PauseManager_OnPausedChangedEvent;
            ASyncLoader.OnLoadingChanged += ASyncLoader_OnLoadingChanged;
        }

        private void ASyncLoader_OnLoadingChanged(object sender, Events.BoolEventArgs e)
        {
            if (e.i) SaveCurrentGame();
        }

        private void PauseManager_OnPausedChangedEvent(object sender, EventArgs e)
        {
            if (!PauseManager.IsPaused)
            {
                SaveCurrentSettings();
            }
        }

        private void OnDestroy()
        {
            PauseManager.OnPausedChangedEvent -= PauseManager_OnPausedChangedEvent;
        }

        #endregion

        #region Game

        public static void InitGame()
        {
            GameData data = new GameData()
            {
                money = 1000
            };

            //saving our new convoy
            SaveGame(data);
        }

        public static void LoadGame()
        {
            //asking for the save string from teh save manager
            string saveString = SaveManager.Load(SaveManager.SAVE_FILE);

            if (saveString != null)
            {
                //creating a save object from the json/string
                GameSaveObject saveObject = JsonUtility.FromJson<GameSaveObject>(saveString);

                GameData data = saveObject.gameData;

                OnGameLoaded(data);
            }

            //if we have no convoy data, initialise it!
            else
            {
                InitGame();
            }
        }

        public static void SaveGame(GameData data)
        {
            //creating a new save object, settting the values
            GameSaveObject saveObject = new GameSaveObject
            {
                gameData = data
            };

            //using json utilities to write a json file
            string json = JsonUtility.ToJson(saveObject, true);

            //calling the save method on the save manager
            SaveManager.Save(SaveManager.SAVE_FILE, json);

            OnGameChanged(data);
        }

        public void SaveCurrentGame()
        {
            GameData data = new GameData()
            {
                money = MoneyManager.GetCurrentMoney()
            };

            SaveGame(data);
        }

        #endregion

        #region Convoy

        public static void InitConvoy()
        {
            List<VehicleData> vdl = new List<VehicleData>();

            //create a new convoy save
            ConvoyData data = new ConvoyData
            {
                vehicleDatas = vdl
            };

            //saving our new convoy
            SaveConvoy(data);
        }

        public static void LoadConvoy()
        {
            //asking for the save string from teh save manager
            string saveString = SaveManager.Load(SaveManager.CONVOY_SAVE_FILE);

            if (saveString != null)
            {
                //creating a save object from the json/string
                ConvoySaveObject saveObject = JsonUtility.FromJson<ConvoySaveObject>(saveString);

                ConvoyData data = saveObject.convoyData;

                OnConvoyLoaded(data);
            }

            //if we have no convoy data, initialise it!
            else
            {
                InitConvoy();
            }
        }

        public static void SaveConvoy(ConvoyData data)
        {
            //creating a new save object, settting the values
            ConvoySaveObject saveObject = new ConvoySaveObject
            {
                convoyData = new ConvoyData { vehicleDatas = data.vehicleDatas }
            };

            //using json utilities to write a json file
            string json = JsonUtility.ToJson(saveObject, true);

            //calling the save method on the save manager
            SaveManager.Save(SaveManager.CONVOY_SAVE_FILE, json);

            OnConvoyChanged(data);
        }

        public void SaveCurrentConvoy()
        {
            List<VehicleData> vdl = new List<VehicleData>();

            foreach (Vehicle vh in ConvoyManager.vehiclesList)
            {
                VehicleData vd = new VehicleData()
                {
                    vehicleHealth = vh.Target.GetCurrentHealth(),
                    chassisIndex = vh.ChassisManager.GetCurrentPartIndex(),
                    turretIndex = vh.ChassisManager.TurretManager.GetCurrentPartIndex()
                };

                vdl.Add(vd);
            }

            ConvoyData data = new ConvoyData()
            {
                vehicleDatas = vdl
            };

            SaveConvoy(data);
        }

        #endregion

        #region Settings

        public static void InitSettings()
        {
            //create a new settings save
            SettingsData data = new SettingsData
            {
                resolutionIndex = 0,
                brightness = 1f, isFullscreen = true, qualityIndex = QualitySettings.GetQualityLevel(),

                masterVolume = 20f,
                ambienceVolume = -20f,
                gameVolume = -20f,
                uiVolume = -30f,
                musicVolume = 0f
            };

            //saving our new settings
            SaveSettings(data);
        }

        public static void LoadSettings()
        {
            //asking for the save string from teh save manager
            string saveString = SaveManager.Load(SaveManager.SETTINGS_SAVE_FILE);

            if (saveString != null)
            {
                //creating a save object from the json/string
                SettingsSaveObject saveObject = JsonUtility.FromJson<SettingsSaveObject>(saveString);

                SettingsData data = saveObject.data;

                OnSettingsLoaded(data);
            }

            //if we have no settings, initialise!
            else
            {
                InitSettings();
            }
        }

        public void SaveCurrentSettings()
        {
            SettingsData data = new SettingsData
            {
                resolutionIndex = VideoSettings.resolutionIndex,
                brightness = VideoSettings.brightness,
                isFullscreen = VideoSettings.isFullscreen,
                qualityIndex = VideoSettings.qualityIndex,

                masterVolume = settings.AudioSettings.masterVolume,
                ambienceVolume = settings.AudioSettings.ambienceVolume,
                musicVolume = settings.AudioSettings.musicVolume,
                gameVolume = settings.AudioSettings.gameVolume,
                uiVolume = settings.AudioSettings.uiVolume,
            };

            SaveSettings(data);
        }

        public static void SaveSettings(SettingsData data)
        {
            //creating a new save object, settting the values
            SettingsSaveObject saveObject = new SettingsSaveObject
            {
                data = data
            };

            //using json utilities to write a json file
            string json = JsonUtility.ToJson(saveObject, true);

            //calling teh save method on the save manager
            SaveManager.Save(SaveManager.SETTINGS_SAVE_FILE, json);

            OnSettingsChanged(data);
        }

        #endregion
    }

    #region Settings

    [Serializable]
    public class SettingsSaveObject
    {
        public SettingsData data;
    }

    [Serializable]
    public struct SettingsData
    {
        //video
        public int resolutionIndex; public float brightness; public bool isFullscreen; public int qualityIndex;

        //audio
        public float masterVolume, ambienceVolume, gameVolume, uiVolume, musicVolume;
    }

    #endregion

    [Serializable]
    public class InventorySaveObject
    {
        public InventoryData inventoryData;
    }

    [Serializable]
    public struct InventoryData
    {
        public int ifvChassisAmount, lrvChassisAmount;

        public int ifvMGTurretAmount, ifvMortarTurretAmount;
        public int lrvMGTurretAmount, lrvMortarTurretAmount;
    }

    [Serializable]
    public class GameSaveObject
    {
        public GameData gameData;
    }

    [Serializable]
    public struct GameData
    {
        public int money;
    }

    [Serializable]
    public class ConvoySaveObject
    {
        public ConvoyData convoyData;
    }

    [Serializable]
    public struct ConvoyData
    {
        public List<VehicleData> vehicleDatas;
    }

    [Serializable]
    public struct VehicleData
    {
        public int vehicleHealth;
        public int chassisIndex, turretIndex;
    }
}