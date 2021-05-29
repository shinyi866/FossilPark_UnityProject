using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFruitPlane : MonoBehaviour
{
    public static int fruitTouchPlane;
    private bool enter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Fruit" && !enter)
        {
            fruitTouchPlane++;
            enter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        enter = false;
    }
}
