using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedDinosaur : MonoBehaviour
{
    public int DinosaurNumber;
    public GameObject[] Dinosaurs;
    public Transform addPos;
    static public GameObject food;
    public GameObject[] foodPrefab;
    public delegate void stfood();
    public static event stfood _stfood;
    // Start is called before the first frame update
    void Start()
    {
        Dinosaurs[DinosaurNumber].SetActive(true);
        addPos.SetParent(Camera.main.transform);
        
        addPos.localPosition = Vector3.zero;
        addPos.rotation = Camera.main.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addFood(int i)
    {
        //food = Instantiate(foodPrefab[i], addPos.GetChild(0).position, addPos.rotation);
        _stfood();
    }
}
