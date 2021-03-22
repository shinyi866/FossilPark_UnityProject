using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

public class ClickGame : MonoBehaviour
{
    [Header("遊玩中影片")]
    public VideoClip vc;//360
    [Header("過關影片")]
    [Tooltip("放置過關時要撥的影片")]
    public VideoClip vc2;
    [Header("限時幾秒")]
    public float OverTime = 10;

    float VideoTime = 12;
    [Header("計時UI")]
    public Text timeUI;
    float f;
    [Header("進度條")]
    public RectTransform strip;
    [Header("成功事件")]
    public UnityEvent completedEvent;
    [Header("失敗事件")]
    public UnityEvent OverEvent;

    public VideoPlayer vp;
    public GameObject gameUI;
    
    void Start()
    {
        if (vp!=null)
        {
            vp.clip = vc;
            vp.Play();
        }
    }

    public void UI_Enter()
    {
        if (f < 10) { f++; }
    }

    void Update()
    {
        
        if (f >= 20)
        {
            gameUI.SetActive(false);
            if (vp != null)
            {
                vp.clip = vc2;
                vp.Play();
            }
            completedEvent.Invoke();

            if (VideoTime > 0)
            {
                VideoTime -= Time.deltaTime;
            }

            if (VideoTime < 0)
            {
                if (vp != null)
                {
                    vp.Pause();
                }
                
            }
        }
        else
        {
            if (f > 0)
            {
                f -= Time.deltaTime * 3;

            }
            if (OverTime > 0)
            {
                OverTime -= Time.deltaTime;
                timeUI.text = Mathf.RoundToInt(OverTime).ToString();
                strip.localScale = new Vector3(0 + (f / 20), 1, 1);
            }
            else
            {
                OverEvent.Invoke();
            }
        }
        
    }

}
