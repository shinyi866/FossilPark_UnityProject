using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameMission;

public class ObjectClick : MonoBehaviour
{
    [SerializeField]
    private Game0 game;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform != null)
                {
                    game.ShowInfo(hit.transform.gameObject);
                }
            }
        }
    }
}
