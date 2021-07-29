using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using View;

public static class PlayerPersistent
{
    public static PlayerData playerData = new PlayerData();
    
    public static void SaveData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        PlayerData data = new PlayerData();
        data.missions = playerData.missions;

        bf.Serialize(file, data);
        file.Close();
    }

    public static void LoadData()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            playerData.missions = data.missions;

            for (int i = 0; i < playerData.missions.Length; i++)
            {
                Debug.Log("i playerData.mission:  " + playerData.missions[i]);
                if(playerData.missions[i])
                {
                    iBeaconMissionSetting.Instance.IbeaconLoadData(i);
                    Modals.instance.GetModel<MainModal>().GetBackAnimal(i);
                }
            }
        }
    }
}

[System.Serializable]
public class PlayerData
{
    public bool[] missions = { false, false, false, false, false, false, false, false, false };
}