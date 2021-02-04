using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogDatabase : MonoBehaviour
{
    [SerializeField]
    private GameDialogData _gameData;
    public static GameDialogData gameData;

    private void Awake()
    {
        gameData = _gameData;
    }
}
