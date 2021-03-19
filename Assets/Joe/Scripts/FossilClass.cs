using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FossilClass : MonoBehaviour
{

    [Header("成功視窗")]
    public GameObject sc;
    Text st;
    //public Texture texture;
    [Header("訊息視窗")]
    public GameObject image;
    //說明文字
    Text dt; 

    [Header("撿起按鈕")]
    public GameObject bt;

    private Camera _camera;
    [Header("成功事件")]
    public UnityEvent completedEvent;
    [Header("失敗事件")]
    public UnityEvent OverEvent;

    public LayerMask HitLayer;
    RaycastHit hit;
    int lack = 3;
    private void Start()
    {
        _camera = Camera.main;
        dt = image.transform.Find("Text").GetComponent<Text>();
        st = sc.transform.Find("Text").GetComponent<Text>();


    }

    private void Update()
    {


    }

    void FixedUpdate()
    {
        
       

        
        
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, Mathf.Infinity, HitLayer))
        {
            image.SetActive(true);
            bt.SetActive(false);
            image.transform.position = _camera.WorldToScreenPoint(hit.collider.transform.position);
            switch (hit.collider.name)
            {
                case "鹿":
                    dt.text = "這是梅花鹿的化石";
                    bt.SetActive(true);
                    break;
                case "貝殼":
                    dt.text = "這是貝殼的化石";
                    break;
                case "犀牛":
                    dt.text = "這是犀牛的化石";
                    break;

            }
            
        }
        else
        {
            image.SetActive(false);
        }
    }

    public void UI_Enter()
    {
        sc.SetActive(true);
        hit.collider.gameObject.SetActive(false); 
        lack--;
        if (lack == 0)
        {
            st.text = "恭喜你完成目標";
            lack = 3;
        }
        else
        {
            st.text = "恭喜你撿到一塊鹿的化石，還差"+lack.ToString()+"塊";
        }
        

    }


}
