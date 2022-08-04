using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine;
using GameMission;
using View;

public class ARImageTrackManager : Singleton<ARImageTrackManager>
{
    [HideInInspector]
    public bool isEnterGame;

    [SerializeField]
    private ARTrackedImageManager ARTrackedImage;

    private string currentImageName;
    private GameMissions gameMissions;
    private Game8 game8;
    private int currentShow;

    private void Awake()
    {
        gameMissions = GameMissions.instance;
        game8 = Games.instance.GetGame<Game8>();
    }

    private void OnEnable()
    {
        ARTrackedImage.trackedImagesChanged += OnTrackedImagesStart; //turn to ibeacon
    }

    private void OnDisable()
    {
        ARTrackedImage.trackedImagesChanged -= OnTrackedImagesStart; //turn to ibeacon
    }

    private void OnTrackedImagesStart(ARTrackedImagesChangedEventArgs eventArgs)
    {

        foreach (ARTrackedImage trackImage in eventArgs.added)
        {
            UpdateARMissionImage(trackImage);
        }

        foreach (ARTrackedImage trackImage in eventArgs.updated)
        {
            UpdateARMissionImage(trackImage);
        }

        foreach (ARTrackedImage trackImage in eventArgs.removed)
        {
            GameModals.instance.OpenModal<ARGameModal>().ShowModal(0, TypeFlag.ARGameType.ARImageTrack);
        }
    }

    private void UpdateARMissionImage(ARTrackedImage trackImage)
    {
        currentImageName = trackImage.referenceImage.name;
        var imagePosition = trackImage.transform.position;
        Debug.Log("================ currentImageName: " + currentImageName);        

        if (!isEnterGame)
        {
            TypeFlag.ImageTrackMissions reault = (TypeFlag.ImageTrackMissions)System.Enum.Parse(typeof(TypeFlag.ImageTrackMissions), currentImageName);
            string reaultString = reault.ToString();
            int mission = int.Parse(reaultString.Substring(reaultString.Length - 1));

            if (currentShow == mission) return;

            for (int i = 0; i < PlayerPersistent.playerData.missions.Length - 1; i++)
            {
                if (i == mission && PlayerPersistent.playerData.missions[i])
                {
                    GameModals.instance.GetModal<TitleModal>().Show(false);
                    GameModals.instance.OpenModal<ARGameModal>().ShowModal(0, TypeFlag.ARGameType.ARImageTrack);
                    return;
                }
            }
            Debug.Log("================ mission: " + mission);
            gameMissions.ShowMission(mission);
            currentShow = mission;

        }
        else
        {
            if (gameMissions.currentIndex != 8) return;
            Debug.Log("================ mission8 ");
            if (game8.dinosaurlScenes != null)
            {
                game8.currentImageName = currentImageName;

                if (game8.currentImageName != "ticket3")
                {
                    GameObject showARObject = game8.arObjects[game8.currentImageName];
                    showARObject.SetActive(true);
                    showARObject.transform.position = imagePosition;

                    game8.showARfood = game8.arObjects[game8.currentImageName];
                    Debug.Log("================ ticket3 ");
                    foreach (GameObject b in game8.arObjects.Values)
                    {
                        Debug.Log($"Show in arObjects.Values: {b.name}");
                        if (b.name != game8.currentImageName)
                        {
                            b.SetActive(false);
                        }
                    }

                    if (Game8.isEat) { showARObject.SetActive(false); }
                }
                else
                {
                    game8.currentImageName = game8.foodGameObject[game8.currentIndex].name;
                    GameObject showARObject = game8.arObjects[game8.currentImageName];
                    showARObject.SetActive(true);
                    showARObject.transform.position = imagePosition;

                    game8.showARfood = game8.arObjects[game8.currentImageName];

                    foreach (GameObject b in game8.arObjects.Values)
                    {
                        Debug.Log($"Show in arObjects.Values: {b.name}");
                        if (b.name != game8.currentImageName)
                        {
                            b.SetActive(false);
                        }
                    }

                    if (Game8.isEat) { showARObject.SetActive(false); }
                }
            }
        }
    }
}
