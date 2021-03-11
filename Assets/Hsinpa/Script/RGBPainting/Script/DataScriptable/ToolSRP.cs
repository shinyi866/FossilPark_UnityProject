using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Tools", order = 1)]
public class ToolSRP : ScriptableObject
{
    public Tool[] tools;

    public enum ToolEnum { 
        Tool_1, Tool_2, Tool_3
    }


    [Serializable]
    public struct Tool {
        public string tool_id;
        public Color mask_color;
        public AudioClip audioClip;

        [Range(0, 0.02f)]
        public float difficulty;

        public bool isValid => !string.IsNullOrEmpty(tool_id);
    }
}