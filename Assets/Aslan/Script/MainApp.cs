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
    private MainGuideData _guideData;
    public MainGuideData guideData => _guideData;

    [HideInInspector]
    public bool isARsupport;

    public Text text;
    public bool arModel;

    private int playerGuide;

    private void Awake()
    {
        StartCoroutine(CheckARSupport());
    }

    private void Start()
    {
        var modal = Modals.instance.OpenModal<MainModal>();

        //Application.lowMemory += OnLowMemory;
        playerGuide = PlayerPrefs.GetInt("guide"); // 0: strat guide view, 1: main view
        
        if (playerGuide != 1)
            modal.StarIntroView();
        else
            modal.StarMainView();
    }

    private IEnumerator CheckARSupport()
    {
        Debug.Log("Checking for AR support...");

        yield return ARSession.CheckAvailability();

        switch (ARSession.state)
        {
            case ARSessionState.Unsupported:
                isARsupport = false;
                text.text = "supportResult: Unsupported";
                break;

            case ARSessionState.None:
                isARsupport = false;
                text.text = "supportResult: None";
                break;

            case ARSessionState.NeedsInstall:
                isARsupport = false;
                text.text = "supportResult: NeedsInstall";
                break;

            case ARSessionState.CheckingAvailability:
                isARsupport = false;
                text.text = "supportResult: CheckingAvailability";
                break;

            case ARSessionState.Installing:
                isARsupport = false;
                text.text = "supportResult: Installing";
                break;

            case ARSessionState.SessionInitializing:
                isARsupport = false;
                text.text = "supportResult: SessionInitializing";
                break;

            case ARSessionState.SessionTracking:
                isARsupport = false;
                text.text = "supportResult: SessionTracking";
                break;

            case ARSessionState.Ready:
                isARsupport = true;
                text.text = "supportResult: Ready";
                break;

            default:
                isARsupport = false;
                break;
        }

        if (arModel) { isARsupport = true; }
        
    }

    /*** 石尚 maybe use socre system

    [SerializeField]
    private ScoreImage scoreObject;

    private int score = 0;

    public void Score()
    {
        var modal = Modals.instance.GetModel<MainModal>();
        var image = modal.scoreImage;

        score++;

        if(score < scoreObject.ScoreItems.Length)
            image.sprite = scoreObject.ScoreItems[score].ScoreImage;
    }
    */
}
