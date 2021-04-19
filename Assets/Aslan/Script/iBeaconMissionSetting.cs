using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameMission;
using View;

public class iBeaconMissionSetting : Singleton<iBeaconMissionSetting>
{
    [SerializeField]
    private MissionRange[] ranges;
    [SerializeField]
    private Transform container;
    [SerializeField]
    private Transform setTransform;

    private List<GameObject> cloneTransform = new List<GameObject>();

    public bool isEnterGame;

    private void Start() { SetPosition(); }

    /* search mission */
    public void MissionSearch(List<Beacon> mybeacons)
	{
        if (mybeacons == null || isEnterGame) return;

		foreach (Beacon b in mybeacons)
		{
			int mission = b.minor;

            for(int i = 0; i < ranges.Length; i++)
            {
                if(mission == i)
                {
                    if (ranges[i].minRange < 0 && ranges[i].maxRange < 0) return;

                    if (b.accuracy < ranges[i].minRange)
					{
						GameMissions.instance.ShowMission(mission);
					}
					else if (b.accuracy > ranges[i].minRange && b.accuracy < ranges[i].maxRange)
					{
						GameModals.instance.RoundNotify(mission);
                    }
					else if(b.accuracy > ranges[i].maxRange)

                    {
						GameModals.instance.CloseModal();
					}
				}
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
                    Texts[2].text = "Min: " + ranges[j].maxRange;
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
                    ranges[i].minRange = double.Parse(inputValue[0].text);
                    ranges[i].maxRange = double.Parse(inputValue[1].text);
                }

                RefreshValue();
                inputValue[0].text = null;
                inputValue[1].text = null;
            }
        }
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
