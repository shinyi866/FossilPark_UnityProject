using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAlert : MonoBehaviour
{
    [SerializeField]
    private GameObject alert;

    private void OnTriggerEnter(Collider target)
    {
        if (target.tag == "MainCamera")
        {
            alert.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        alert.SetActive(false);
    }
}
