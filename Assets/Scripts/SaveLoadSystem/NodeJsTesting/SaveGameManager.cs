using System.IO;
using UnityEngine;
using UnityEngine.Events;


namespace SaveLoadSystem
{
    public static class SaveGameManager
    {
        public static SaveData currentSaveData = new SaveData();

        public const string saveDirectory = "/SaveData/";
        public const string FileName = "SaveGame.sav";

        public static UnityAction OnloadGameStart;
        public static UnityAction OnloadGameFinish;

        public static bool SaveGame()
        {

            var dir = Application.persistentDataPath + saveDirectory;

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string json = JsonUtility.ToJson(currentSaveData, true);
            File.WriteAllText(dir + FileName, json);

            GUIUtility.systemCopyBuffer = dir;

            return true;
        }

        public static void LoadGame()
        {
            OnloadGameStart?.Invoke();
            string fullPath = Application.persistentDataPath + saveDirectory + FileName;
            SaveData tempData = new SaveData();

            if (File.Exists(fullPath))
            {
                string json = File.ReadAllText(fullPath);
                tempData = JsonUtility.FromJson<SaveData>(json);
            }
            else
            {
                Debug.LogError("Save file does not exist");
            }

            currentSaveData = tempData;
            OnloadGameFinish?.Invoke();
        }
    }
}
