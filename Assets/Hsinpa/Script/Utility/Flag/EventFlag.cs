using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventFlag 
{
    public class Event
    {
        public const string GameStart = "event@game_start";
        public const string GameRestart = "event@game_restart";

        public const string OnRhenoARMode_PlaneAR = "event@planeAR_activate";
        public const string OnRhenoARMode_SpatialAR = "event@spatialAR_activate";
        public const string OnRhenoARMode_NoAR= "event@noAR_activate";

        public const string OnCrocodileARMode_PlaneAR = "event@crocodile_armode";
        public const string OnCrocodileARMode_NoAR = "event@crocodile_noar";
    }
}