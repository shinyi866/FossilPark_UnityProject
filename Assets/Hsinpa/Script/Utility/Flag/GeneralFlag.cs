﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralFlag
{
    public class Layer {
        public const int Plane = 1 << 20;
        public const int ARDetectable = 1 << 21;
        public const int IgnoreRaycast = 1 << 2;

        public const int PlaneInt = 20;
        public const int ARDetectableInt = 21;
        public const int IgnoreRaycastInt = 2;
    }

    public class MissionID {
        public const string BoneRepairHome = "mission.home.bone_repair";
        public const string BoneRepairHTC = "mission.htc.bone_repair";
        public const string BoneRepair = "mission.bone_repair";

        public const string FrontEgg =  "mission.front_egg";
        public const string LobbyDeer = "mission.lobby_deer";
    }

    public enum GeneralState
    {
        Idle,
        Preparation,
        UnderGoing
    }

    public class MatPropertyName {
        public const string Color = "_Color";
    }

    public enum AnchorType { 
        Position,
        Support,
        Text
    }

    public enum BoneType {
        Idle,
        Selected,
        Locked,
        TemplateIdle,
        TemplateHint,
        TemplateLocked
    }
}
