using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using RootMotion.FinalIK;

public class dinosaurAI : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public float eatDistance;
    public Transform target;
    Vector3 targetPos;
    public GameObject food;
    public CCDIK chainIK;
    Animator ani;
    public bool meat;
   
    void Start()
    {
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ani.SetBool("Idle", target==null);
        ani.SetBool("Walk", target != null);
        if (target)
        {
            targetPos.Set(target.position.x, transform.position.y, target.position.z);
            transform.LookAt(targetPos);
            if (Vector3.Distance(targetPos, transform.position) > eatDistance)
            {
                transform.Translate(speed * transform.forward * Time.deltaTime, Space.World);
                ani.SetBool("Eat", false);
            }
            else
            {
                ani.SetBool("Eat", true);
                chainIK.solver.target.position = target.position+new Vector3(0,0.5f,0);
                chainIK.solver.IKPositionWeight += 1 * Time.deltaTime;
                if (chainIK.solver.IKPositionWeight > 1)
                {
                    chainIK.solver.IKPositionWeight = 1;
                }
            }
        }
        else
        {
            ani.SetBool("Eat", false);
            chainIK.solver.IKPositionWeight -= 1 * Time.deltaTime;
            if (chainIK.solver.IKPositionWeight < 0)
            {
                chainIK.solver.IKPositionWeight = 0;
            }
        }
       
        
        
    }

    public void eat()
    {
        
        if (food.transform.childCount == 1)
        {

            Destroy(food);
            food = null;
            target = null;
        }
        else
        {
            Destroy(food.transform.GetChild(0).gameObject);
        }
      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (meat) {
            if (other.tag == "Meat")
            {
                food = other.gameObject;
                target = food.transform;
            }
        }
        else
        {
            if (other.tag == "Food")
            {
                food = other.gameObject;
                target = food.transform;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Food")
        {
            food = null;
            
        }
    }
}
