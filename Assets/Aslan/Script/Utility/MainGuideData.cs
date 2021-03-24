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
        public Sprite mainPicture1;
        public Sprite mainPicture2;
        public Sprite mainPicture3;
    }

    public GuideData[] m_Data;
}
