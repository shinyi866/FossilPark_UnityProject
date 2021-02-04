using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDialogData : ScriptableObject
{
    [System.Serializable]
    public class Data
    {
        public int missionID;
        public string roundMessage;
        public string enterMessage;
        public string enterButton;
        public string gameDescription;
        public string dialogButton;
        public string[] gameNotify;
        public string success;
        public string fail;
        public string pictureNotify;

        public Sprite animalDialogPicture;
        public Sprite animalNotifyPicture;
    }

    public Data[] m_Data;
}
