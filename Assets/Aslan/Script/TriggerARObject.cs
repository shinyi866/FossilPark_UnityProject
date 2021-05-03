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
            target = null;
            Game8.isEat = true;
            foodInMouth.SetActive(true);
            target.transform.parent.gameObject.SetActive(false);
            StartCoroutine(WaitToEat());
            Debug.Log("eating");
        }
    }

    private IEnumerator WaitToEat()
    {
        yield return new WaitForSeconds(7);

        foodInMouth.SetActive(false);
        Game8.isEat = false;
    }
}
