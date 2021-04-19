using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedDinosaur : MonoBehaviour
{
    public Transform addPos;
    static public GameObject food;
    public GameObject[] foodPrefab;
    // Start is called before the first frame update
    void Start()
    {

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
        food = Instantiate(foodPrefab[i], addPos.GetChild(0).position, addPos.rotation);
    }
}
