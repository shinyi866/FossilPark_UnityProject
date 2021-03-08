using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;
using UnityEngine.XR.ARFoundation;

public class MainApp : Singleton<MainApp>
{
    protected MainApp() { } // guarantee this will be always a singleton only - can't use the constructor!

    [SerializeField]
    private GameDialogData _database;
    public GameDialogData database => _database;

    [SerializeField]
    private ScoreImage scoreObject;

    [HideInInspector]
    public bool isARsupport;

    public Text text;
    public bool arModel;

    private int score = 0;

    private void Awake()
    {
        StartCoroutine(CheckARSupport());
    }

    private void Start()
    {
        var modal = Modals.instance.OpenModal<MainModal>();
        modal.StarMainView();
    }

    public void Score()
    {
        var modal = Modals.instance.GetModel<MainModal>();
        var image = modal.scoreImage;

        score++;

        if(score < scoreObject.ScoreItems.Length)
            image.sprite = scoreObject.ScoreItems[score].ScoreImage;
    }

    private IEnumerator CheckARSupport()
    {
        Debug.Log("Checking for AR support...");

        yield return ARSession.CheckAvailability();        

        switch (ARSession.state)
        {
            case ARSessionState.Unsupported:
                isARsupport = false;
                Debug.Log("supportResult: Unsupported");
                text.text = "supportResult: Unsupported";
                break;

            case ARSessionState.None:
                isARsupport = false;
                Debug.Log("supportResult: None");
                text.text = "supportResult: None";
                break;

            case ARSessionState.NeedsInstall:
                isARsupport = false;
                Debug.Log("supportResult: NeedsInstall");
                text.text = "supportResult: NeedsInstall";
                break;

            case ARSessionState.Ready:
                isARsupport = true;
                Debug.Log("supportResult: Ready");
                text.text = "supportResult: Ready";
                break;

            default:
                isARsupport = false;
                break;
        }

        if (arModel) { isARsupport = true; }
        
    }
}
