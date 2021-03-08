using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameMission;

public class TriggerARObject : MonoBehaviour
{
    public GameObject foodInMouth;
    public Game8 mission;

    void OnTriggerEnter(Collider target)
    {
        if (target.tag == "Fruit")
        {
            mission.isEat = true;
            foodInMouth.SetActive(true);
            Debug.Log("eating");
        }
    }
}
