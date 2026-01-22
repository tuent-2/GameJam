using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using UnityEngine;

    [Serializable]
    public class ProjectDataDynamic : ProjectSingleton<ProjectDataDynamic>
    {
        public ProjectDataDynamic_Setting setting;
        public int iCurrentLevel;
    }

    [Serializable]
    public struct ProjectDataDynamic_Setting
    {
        public bool sound;
        public bool music;
        public bool vibrate;
    }
