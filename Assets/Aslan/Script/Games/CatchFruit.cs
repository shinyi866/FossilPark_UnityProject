using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchFruit : MonoBehaviour
{
    public static int fruitCount;
    private bool enter;
    //public GameObject basket;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Fruit" && !enter)
        {
            fruitCount++;
            enter = true;
        }

        if (other.gameObject.tag == "Wall1")
        {
            Debug.Log("wall1");
            this.transform.position = new Vector3(-0.75f, this.transform.position.y, this.transform.position.z);
        }

        if (other.gameObject.tag == "Wall2")
        {
            Debug.Log("wall1");
            this.transform.position = new Vector3(0.75f, this.transform.position.y, this.transform.position.z);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        enter = false;
    }
}
