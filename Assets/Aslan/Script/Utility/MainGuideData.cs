using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGuideData : ScriptableObject
{

    [System.Serializable]
    public class GuideData
    {
        public int missionID;
        public string[] introMessage;
        public string[] gamePrompt;

        public Sprite guideAnimal;
        public Sprite guidePicture;
    }

    public GuideData[] m_Data;
}
