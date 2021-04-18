using UnityEngine;

namespace Hsinpa.View
{
    public class ToolItemSwitcher : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] toolHolders;

        public void EnableTool(int index)
        {
            for (int i = 0; i < toolHolders.Length; i++)
            {
                toolHolders[i].SetActive(index == i);
            }
        }

        public void HideAll()
        {
            for (int i = 0; i < toolHolders.Length; i++)
            {
                toolHolders[i].SetActive(false);
            }
        }
    }
}