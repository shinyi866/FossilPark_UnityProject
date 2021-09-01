using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine;
using GameMission;

public class ARImageTrackManager : Singleton<ARImageTrackManager>
{
    [HideInInspector]
    public bool isEnterGame;

    [SerializeField]
    private ARTrackedImageManager ARTrackedImage;

    private string currentImageName;
    private GameMissions gameMissions;
    private Game8 game8;

    private void Awake()
    {
        gameMissions = GameMissions.instance;
        game8 = Games.instance.GetGame<Game8>();
    }

    private void OnEnable()
    {
        //if (gameMissions.currentIndex == 8)
          //  ARTrackedImage.trackedImagesChanged += Games.instance.OpenGame<Game8>().Game8TrackedImagesStart;
        //else
            ARTrackedImage.trackedImagesChanged += OnTrackedImagesStart;
    }

    private void OnDisable()
    {
        //if (gameMissions.currentIndex == 8)
          //  ARTrackedImage.trackedImagesChanged -= Games.instance.OpenGame<Game8>().Game8TrackedImagesStart;
        //else
            ARTrackedImage.trackedImagesChanged -= OnTrackedImagesStart;
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
    }

    private void UpdateARMissionImage(ARTrackedImage trackImage)
    {
        currentImageName = trackImage.referenceImage.name;
        var imagePosition = trackImage.transform.position;

        Debug.Log("=========================== currentImageName: " + currentImageName);        

        if(gameMissions.currentIndex == 8)
        {
            if (!isEnterGame) return;

            if (game8.dinosaurlScenes != null)
            {
                game8.currentImageName = currentImageName;

                if (game8.currentImageName != "ticket3")
                {
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
        else
        {
            if(!isEnterGame)
            {
                TypeFlag.ImageTrackMissions reault = (TypeFlag.ImageTrackMissions)System.Enum.Parse(typeof(TypeFlag.ImageTrackMissions), currentImageName);
                string reaultString = reault.ToString();
                int mission = int.Parse(reaultString.Substring(reaultString.Length - 1));

                gameMissions.ShowMission(mission);

                Debug.Log("=========================== reault: " + reault + " mission: " + mission);
            }
        }
    }
}
