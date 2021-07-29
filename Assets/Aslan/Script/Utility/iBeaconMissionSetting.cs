using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameMission;
using View;
using System;

public class iBeaconMissionSetting : Singleton<iBeaconMissionSetting>
{
    [SerializeField]
    private MissionRange[] ranges;
    [SerializeField]
    private MissionRange[] originRanges;
    [SerializeField]
    private Transform container;
    [SerializeField]
    private Transform setTransform;
    [SerializeField]
    private Text text;
    [SerializeField]
    private GameObject alert;

    private List<GameObject> cloneTransform = new List<GameObject>();
    private Beacon minBeacon;

    public bool isEnterGame;
    public bool isLastMissionOpen;

    private void Start()
    {
        SetPosition();
    }

    /* search mission */
    public void MissionSearch(List<Beacon> mybeacons)
	{
        if (mybeacons.Count <= 0 || mybeacons == null || isEnterGame) return;
        
        double minDistance = Mathf.Infinity;
        
        foreach(Beacon b in mybeacons)
        {
            // TODO: search 2,8 mission

            if (b.accuracy > 0 && b.accuracy < minDistance)
            {
                minDistance = b.accuracy;
                minBeacon = b;
            }
        }

        if (minBeacon.minor > 8) return;
        text.text = $"minDistance: {minBeacon.accuracy}, minIndex: {minBeacon.minor}";

        int mission = minBeacon.minor;

        if (ranges[mission].minRange < 0 && ranges[mission].maxRange < 0) return;

        if(minBeacon.major == 0)
        {
            if (minBeacon.accuracy == 8 && !isLastMissionOpen) return; // Finish all mission will open

            if (minBeacon.accuracy < ranges[mission].minRange)
            {
                Handheld.Vibrate();
                //SoundPlayerController.Instance.AlertSoundEffect();
                GameMissions.instance.ShowMission(mission);        
            }
            else if (minBeacon.accuracy > ranges[mission].minRange && minBeacon.accuracy < ranges[mission].maxRange)
            {
                GameModals.instance.RoundNotify(mission);
            }
            else if (minBeacon.accuracy > ranges[mission].maxRange)
            {
                GameModals.instance.CloseModal();
            }
        }
        else
        {
            Debug.Log("alert");
            if (minBeacon.accuracy < 0.55)
            {
                alert.SetActive(true);
            }
            else
            {
                alert.SetActive(false);
            }
        }

    }

    private void SetPosition()
    {
        float height = 62f;

        for (int i = 0; i < ranges.Length; i++)
        {
            Transform missionClone = Instantiate(setTransform, container);
            RectTransform rankRectTransform = missionClone.GetComponent<RectTransform>();
            RectTransform containRect = container.GetComponent<RectTransform>();

            containRect.sizeDelta = new Vector2(0, 400);
            rankRectTransform.anchoredPosition = new Vector2(0, height - height * (i + 1));            

            Text[] Texts = missionClone.GetComponentsInChildren<Text>();

            Texts[0].text = "Mission" + i;
            Texts[1].text = "Min: " + ranges[i].minRange;
            Texts[2].text = "Min: " + ranges[i].maxRange;

            missionClone.gameObject.SetActive(true);
            cloneTransform.Add(missionClone.gameObject);
        }
    }

    private void RefreshValue()
    {
        for (int i = 0; i < cloneTransform.Count; i++)
        {
            for (int j = 0; j < ranges.Length; j++)
            {
                if(i == j)
                {
                    Text[] Texts = cloneTransform[i].GetComponentsInChildren<Text>();
                    Texts[1].text = "Min: " + ranges[j].minRange;
                    Texts[2].text = "Max: " + ranges[j].maxRange;
                }
            }
        }
    }

    public void UpdateMissionRange()
    {
        for (int i = 0; i < ranges.Length; i++)
        {
            for (int j = 0; j < cloneTransform.Count; j++)
            {
                InputField[] inputValue = cloneTransform[j].GetComponentsInChildren<InputField>();

                if (i == j && !string.IsNullOrEmpty(inputValue[0].text) && !string.IsNullOrEmpty(inputValue[1].text))
                {
                    ranges[i].minRange = Convert.ToDouble(inputValue[0].text);
                    ranges[i].maxRange = Convert.ToDouble(inputValue[1].text);

                    inputValue[0].text = null;
                    inputValue[1].text = null;
                }
            }
        }

        RefreshValue();
    }

    public void IbeaconNotDetect(bool isEnter)
    {
        var index = GameMissions.instance.currentIndex;
        isEnterGame = isEnter;

        if(isEnter)
        {
            PlayerPersistent.playerData.missions[index] = true;

            ranges[index].minRange = -1;
            ranges[index].maxRange = -1;
            
            if (index == 0 && !Games.instance.GetGame<Game1>().isMisssionEnd)
            {
                ranges[1].minRange = originRanges[1].minRange;
                ranges[1].maxRange = originRanges[1].maxRange;
            }
        }
        else
        {
            PlayerPersistent.playerData.missions[index] = false;

            ranges[index].minRange = originRanges[index].minRange;
            ranges[index].maxRange = originRanges[index].maxRange;
        }

        PlayerPersistent.SaveData();
        RefreshValue();
    }

    public void IbeaconLoadData(int index)
    {
        ranges[index].minRange = -1;
        ranges[index].maxRange = -1;

        RefreshValue();
    }
}

[System.Serializable]
public class MissionRange
{
	public double minRange;
	public double maxRange;
}
