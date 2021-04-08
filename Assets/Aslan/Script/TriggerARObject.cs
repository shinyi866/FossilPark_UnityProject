using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameMission;

public class TriggerARObject : MonoBehaviour
{
    public GameObject foodInMouth;
    public Game8 mission;

    private void OnTriggerEnter(Collider target)
    {
        if (target.tag == "Fruit")
        {
            mission.isEat = true;
            foodInMouth.SetActive(true);
            StartCoroutine(WaitToEat());
            Debug.Log("eating");
        }
    }

    private IEnumerator WaitToEat()
    {
        yield return new WaitForSeconds(5);

        foodInMouth.SetActive(false);

        yield return new WaitForSeconds(3);
        // Reset eat parameter
        mission.resetEatFood = true;
        mission.isEat = false;
    }
}
