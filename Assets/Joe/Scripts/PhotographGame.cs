using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

public class PhotographGame : MonoBehaviour
{
    [Header("拍攝目標")]
    public GameObject target;

    //public Texture texture;
    [Header("顯示拍到的畫面")]
    public GameObject image;
    
    
    private Camera _camera;
    [Header("成功事件")]
    public UnityEvent completedEvent;
    [Header("失敗事件")]
    public UnityEvent OverEvent;
    private void Start()
    {
        _camera = Camera.main;
        
        
        target.SetActive(true);
        image.SetActive(false);
    }

    private void Update()
    {
        
        
    }

    public void UI_Enter()
    {

        if (Physics.Linecast(_camera.transform.position, _camera.transform.position + (_camera.transform.forward*30000)))
        {
            StartCoroutine(RenderScreenShot());
            completedEvent.Invoke();

        }
        else
        {
            StartCoroutine(RenderScreenShot());
            OverEvent.Invoke();
        }
        
    }

    private IEnumerator RenderScreenShot()
    {
        

        yield return new WaitForSeconds(0.1f);

        _camera.targetTexture = new RenderTexture(222, 128, 0);//Camera.main.pixelWidth, Camera.main.pixelHeight, 0);

        RenderTexture renderTexture = _camera.targetTexture;
        Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
        _camera.Render();
        RenderTexture.active = renderTexture;
        Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);

        renderResult.ReadPixels(rect, 0, 0);
        renderResult.Apply();

        Sprite screenShot = Sprite.Create(renderResult, rect, Vector2.zero);
        image.GetComponent<Image>().sprite = screenShot;
        image.SetActive(true);

        _camera.targetTexture = null;

        
        
    }
    
}
