using Sirenix.OdinInspector;
namespace Data
{
    using System;
    using UnityEngine;

    public class S_GM_Data_Dynamic : GMModuleBehaviour
    {
        [SerializeField] private string localDataPrefixKey;
        public ProjectDataDynamic dataCurrent;
        private void Awake()
        {
           HandleLocalDataServices.LocalDataPrefix = localDataPrefixKey;
            Application.targetFrameRate = 60;
            DontDestroyOnLoad(this.gameObject);
            this.LoadLocalData();
        }

        private void LoadLocalData()
        {
            var data = HandleLocalDataServices.Load<ProjectDataDynamic>();
            if (data != null)
            {
                Debug.Log("GM_DynamicData -> LoadLocalData -> data not null , now fill");
                
                ProjectDataDynamic.Instance = data;

                dataCurrent = data;
            }
            else
            {
                Debug.Log("GM_DynamicData -> LoadLocalData -> data null , now create one");
                
                ProjectDataDynamic.Instance = dataCurrent;
            }
        }
        

        public static void SaveLocalData()
        {
            HandleLocalDataServices.Save(ProjectDataDynamic.Instance);
        }

        [Button]
        private void SaveCurrentDataAsLocalData()
        {
            HandleLocalDataServices.Save(dataCurrent);
        }

        private void OnApplicationQuit()
        {
            SaveLocalData();
        }

        private void OnApplicationPause(bool isPause)
        {
            Debug.Log($"GM_DynamicData -> OnApplicationPause -> {isPause}");
            //ANR fix: only Save data when focus lost 
            if (isPause)
            {
                Debug.Log($"GM_DynamicData -> OnApplicationPause -> SaveLocalData()");
                SaveLocalData();
            }
        }
    }
}
