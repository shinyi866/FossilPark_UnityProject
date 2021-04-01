using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameMission;

public class TriggerAns : MonoBehaviour
{
    [SerializeField]
    private Game4 game4;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ans")
        {
            Game4.AnsInBoxCount += 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ans")
        {
            Game4.AnsInBoxCount -= 1;
        }
    }
}
