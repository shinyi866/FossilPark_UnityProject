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
            foodInMouth.SetActive(true);
            target.transform.parent.gameObject.SetActive(false);
            StartCoroutine(WaitToEat());
            Game8.currentDinosaurl.GetComponent<Animator>().SetBool("eat", true);
            Game8.isEat = true;
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
