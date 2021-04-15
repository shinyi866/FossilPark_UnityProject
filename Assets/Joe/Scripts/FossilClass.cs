using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using View;

public class FossilClass : MonoBehaviour
{

    //[Header("成功視窗")]
    //public GameObject sc;
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

    private int missionIndex = 2;
    private int currentID;
    private ARGameModal modal;
    public System.Action<bool> gameOverEvent;

    private void Start()
    {
        _camera = CameraCtrl.instance.GetCurrentCamera();
        dt = image.transform.Find("Text").GetComponent<Text>();

        modal = GameModals.instance.GetModal<ARGameModal>();
    }

    private void GameResult(bool isSuccess)
    {
        if (gameOverEvent != null)
            gameOverEvent(isSuccess);
    }

    void FixedUpdate()
    {
        
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, 15, HitLayer))
        {
            image.SetActive(true);
            bt.SetActive(false);
            image.transform.position = _camera.WorldToScreenPoint(hit.collider.transform.position);
            switch (hit.collider.name)
            {
                case "鹿":
                    dt.text = "這是鹿角化石";
                    bt.SetActive(true);
                    currentID = 1;
                    break;
                case "貝殼":
                    dt.text = "這是劍齒象臼齒的化石";
                    bt.SetActive(true);
                    currentID = 3;
                    break;
                case "犀牛":
                    dt.text = "這是犀牛左下顎的化石";
                    bt.SetActive(true);
                    currentID = 2;
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
        hit.collider.gameObject.SetActive(false); 
        lack--;

        if (currentID == 1)
        {
            modal.game2Panel.findImages[0].enabled = true;
            modal.gamePromptPanel.image.sprite = modal.game2Panel.findImages[0].sprite;
            modal.ShowPrompt(missionIndex, TypeFlag.ARGameType.GamePrompt1);
            modal.ShowModal(missionIndex, TypeFlag.ARGameType.Game2);

            if (lack == 0)
            {
                lack = 3;

                modal.SwitchConfirmButton(true);
                modal.gamePromptPanel.button_confirm.onClick.AddListener(() =>
                {
                    modal.ShowPanel(modal.gamePromptPanel.canvasGroup, false);
                    GameResult(true);
                });
            }
        }

        if (currentID == 2)
        {
            modal.game2Panel.findImages[1].enabled = true;
            modal.gamePromptPanel.image.sprite = modal.game2Panel.findImages[1].sprite;
            modal.ShowPrompt(missionIndex, TypeFlag.ARGameType.GamePrompt2);
            modal.ShowModal(missionIndex, TypeFlag.ARGameType.Game2);

            if (lack == 0)
            {
                lack = 3;

                modal.SwitchConfirmButton(true);
                modal.gamePromptPanel.button_confirm.onClick.AddListener(() =>
                {
                    modal.ShowPanel(modal.gamePromptPanel.canvasGroup, false);
                    GameResult(true);
                });
            }
        }

        if (currentID == 3)
        {
            modal.game2Panel.findImages[2].enabled = true;
            modal.gamePromptPanel.image.sprite = modal.game2Panel.findImages[2].sprite;
            modal.ShowPrompt(missionIndex, TypeFlag.ARGameType.GamePrompt3);
            modal.ShowModal(missionIndex, TypeFlag.ARGameType.Game2);

            if (lack == 0)
            {
                lack = 3;

                modal.SwitchConfirmButton(true);
                modal.gamePromptPanel.button_confirm.onClick.AddListener(() =>
                {
                    modal.ShowPanel(modal.gamePromptPanel.canvasGroup, false);
                    GameResult(true);
                });
            }
        }
    }


}
