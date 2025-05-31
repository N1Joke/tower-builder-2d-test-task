using Assets._Project.Scripts.UserData;
using Core;
using Newtonsoft.Json;
using UnityEngine;

namespace UserSave
{
    public class UserData : BaseDisposable, ITowerSaver
    {
        public UserData()
        {
            Init();
        }

        [System.Serializable]
        public class ProgressData
        {
            public CubeData[] cubeData;
        }

        private ProgressData progressData = new ProgressData();

        private static string ProgressKey = "ProgressKey";

        #region Data managment       
        private void Init()
        {
            var progressJson = PlayerPrefs.GetString(ProgressKey, "");
            if (progressJson.Length > 2)
                progressData = JsonConvert.DeserializeObject<ProgressData>(progressJson);
        }

        public void SaveProgressData() => SaveData(ProgressKey, JsonConvert.SerializeObject(progressData, Formatting.Indented));
        private void SaveData(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }
        #endregion

        public void SaveTowerData(CubeData[] data)
        {
            progressData.cubeData = data;
            SaveProgressData();
        }

        public CubeData[] GetTowerData()
        {
            return progressData.cubeData;
        }
    }
}