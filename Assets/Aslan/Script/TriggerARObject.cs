using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameMission;

public class TriggerARObject : MonoBehaviour
{
    public GameObject foodInMouth;
    //public Game8 mission;

    private void OnTriggerEnter(Collider target)
    {
        if (target.tag == "Fruit")
        {
            Game8.isEat = true;
            foodInMouth.SetActive(true);
            target.transform.parent.gameObject.SetActive(false);
            StartCoroutine(WaitToEat());
            Debug.Log("eating");
        }
    }

    private IEnumerator WaitToEat()
    {
        yield return new WaitForSeconds(10);

        foodInMouth.SetActive(false);

        yield return new WaitForSeconds(5);
        // Reset eat parameter
        Game8.resetEatFood = true;
        Game8.isEat = false;
    }
}
