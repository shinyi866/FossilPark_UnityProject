using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameMission;

public class ThrowBallEvent : MonoBehaviour
{
    private Game3 game;

    private void Start()
    {
        game = Games.instance.GetGame<Game3>();
    }

    public void ThrowBall()
    {
        game.ThrowOut();
    }
    
}
