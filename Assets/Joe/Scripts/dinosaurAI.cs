using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using RootMotion.FinalIK;

public class dinosaurAI : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public float eatDistance;
   // public Transform target;
    Vector3 targetPos;
    public GameObject food;
    public CCDIK chainIK;
    Animator ani;
    public bool meat;
    public bool eating; 
    void Start()
    {
        ani = GetComponent<Animator>();
        FeedDinosaur._stfood += eatfood;
    }

    // Update is called once per frame
    void Update()
    {
        //ani.SetBool("Idle", target==null);
        //ani.SetBool("Walk", target != null);
        if (eating)
        {
            //targetPos.Set(target.position.x, transform.position.y, target.position.z);
            //transform.LookAt(targetPos);
            if (Vector3.Distance(targetPos, transform.position) > eatDistance)
            {
                //transform.Translate(speed * transform.forward * Time.deltaTime, Space.World);
                //ani.SetBool("Eat", false);
            }
            else
            {
                
            }

            
            //chainIK.solver.target.position = target.position + new Vector3(0, 0.5f, 0);
            chainIK.solver.IKPositionWeight += 0.5f * Time.deltaTime;
            if (chainIK.solver.IKPositionWeight > 1f)
            {
                chainIK.solver.IKPositionWeight = 1f;
            }
        }
        else
        {
            
            chainIK.solver.IKPositionWeight -= 0.5f * Time.deltaTime;
            if (chainIK.solver.IKPositionWeight < 0)
            {
                chainIK.solver.IKPositionWeight = 0;
            }
        }
       
        
        
    }

    public void steat()
    {
        eating= true;
    }
    int foodi = 0; 
    public void eat()
    {
        eating = false;
        if (foodi == 2)
        {

            //Destroy(food);
            food.SetActive(false);
            food.transform.GetChild(0).gameObject.SetActive(true);
            food.transform.GetChild(1).gameObject.SetActive(true);
            food.transform.GetChild(2).gameObject.SetActive(true);
            //food = null;
            // target = null;
            foodi = 0;
            ani.SetBool("Eat", false);
        }
        else
        {

            //Destroy(food.transform.GetChild(0).gameObject);

            food.transform.GetChild(foodi).gameObject.SetActive(false);
            foodi++;
        }
      
    }


    public void eatfood()
    {
        ani.SetBool("Eat", true);
        food.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (meat) {
            if (other.tag == "Meat")
            {
                food = other.gameObject;
                //target = food.transform;
            }
        }
        else
        {
            if (other.tag == "Food")
            {
                food = other.gameObject;
                //target = food.transform;
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
