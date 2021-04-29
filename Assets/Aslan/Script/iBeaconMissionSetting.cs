using System.Collections;
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

    private void Start() { SetPosition(); }

    /* search mission */
    public void MissionSearch(List<Beacon> mybeacons)
	{
        if (mybeacons.Count == 0 || mybeacons == null || isEnterGame) return;
        /*
        foreach (Beacon b in mybeacons)
        {
            int mission = b.minor;
            int limit = b.major;

            if (ranges[mission].minRange < 0 && ranges[mission].maxRange < 0) return;
            if (b.accuracy < 0) return;

            if (b.accuracy < ranges[mission].minRange)
            {
                Handheld.Vibrate();
                SoundPlayerController.Instance.AlertSoundEffect();
                GameMissions.instance.ShowMission(mission);
            }
            else if (b.accuracy > ranges[mission].minRange && b.accuracy < ranges[mission].maxRange)
            {
                GameModals.instance.RoundNotify(mission);
            }
            else if (b.accuracy > ranges[mission].maxRange)
            {
                GameModals.instance.CloseModal();
            }

            if (limit == 1)
            {
                Debug.Log("alert");
                if(b.accuracy < 0.3)
                {
                    alert.SetActive(true);
                }
                else
                {
                    alert.SetActive(false);
                }
                
            }
        }
        */
        
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

        text.text = $"minDistance: {minBeacon.accuracy}, minIndex: {minBeacon.minor}";

        int mission = minBeacon.minor;

        if (ranges[mission].minRange < 0 && ranges[mission].maxRange < 0) return;

        if(minBeacon.major == 0)
        {
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

    public void IBeaconNotDetect(int index)
    {
        isEnterGame = true;

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
