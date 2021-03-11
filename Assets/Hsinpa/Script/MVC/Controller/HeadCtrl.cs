using Hsinpa.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.Other;

namespace Hsinpa.Ctrl
{
    public class HeadCtrl : ObserverPattern.Observer
    {
        [SerializeField]
        private RhinoBoneRepairCtrl _rhinoBoneRepairCtrl;

        public override void OnNotify(string p_event, params object[] p_objects)
        {
            switch (p_event)
            {
                case EventFlag.Event.GameStart:
                {
                    bool testMode = (bool) p_objects[1];

                    if (testMode)
                        ShowSettingModal();
                } break;

            }
        }

        private void ShowSettingModal() {
            DialogueModal dialogueModal = Modals.instance.OpenModal<DialogueModal>();

            dialogueModal.SetDialogue(StringSet.Dialogue.StartModeTitle, StringSet.Dialogue.StartModeContent,
                new string[] { StringSet.Dialogue.CrocodileMissionTitle, StringSet.Dialogue.RhenoMissionTitle },
             (int index) => {
                 if (index == 0)
                     ShowCrocodileDialogue(dialogueModal);
                 else
                     ShowRhenoDialogue(dialogueModal);    
             });
        }

        private void ShowCrocodileDialogue(DialogueModal dialogueModal)
        {
            string[] eventArray = new string[] { EventFlag.Event.OnCrocodileARMode_PlaneAR, EventFlag.Event.OnCrocodileARMode_NoAR };

            dialogueModal.SetDialogue(StringSet.Dialogue.ARModeTitle, StringSet.Dialogue.StartModeContent,
            new string[] { StringSet.Dialogue.StartModePlaneAR, StringSet.Dialogue.StartModeNoAR },
            (int index) => {
                Modals.instance.Close();
                Hsinpa.DinosaurApp.Instance.Notify(eventArray[index]);
            });
        }

        private void ShowRhenoDialogue(DialogueModal dialogueModal) 
        {
            string[] eventArray = new string[] { EventFlag.Event.OnRhenoARMode_PlaneAR, EventFlag.Event.OnRhenoARMode_NoAR };
            dialogueModal.SetDialogue(StringSet.Dialogue.ARModeTitle, StringSet.Dialogue.StartModeContent,
            new string[] { StringSet.Dialogue.StartModePlaneAR, StringSet.Dialogue.StartModeNoAR },
            (int index) => {
                Modals.instance.Close();
                Hsinpa.DinosaurApp.Instance.Notify(eventArray[index]);
            });
        }

    }
}