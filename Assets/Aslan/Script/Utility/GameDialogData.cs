using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDialogData : ScriptableObject
{
    [System.Serializable]
    public class Data
    {
        public int missionID;
        public string[] roundMessage;
        public string enterMessage;
        public string enterButton;
        public string gameDescription;
        public string[] gameTitle;
        public string titleButton;
        public int guideID;
        public string[] gameGuide;
        public string guideButton;
        public string gameNotify;
        public string[] gamePrompt;
        public string[] endMessage;
        public string pictureNotify;

        public Sprite animalDialogPicture;
        public Sprite animalNotifyPicture;
        public Sprite guidePicture1;
        public Sprite guidePicture2;
        public Sprite ARmark;
    }

    public Data[] m_Data;
}
