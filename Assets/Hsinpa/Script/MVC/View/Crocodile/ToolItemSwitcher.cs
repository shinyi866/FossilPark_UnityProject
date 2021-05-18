using UnityEngine;

namespace Hsinpa.View
{
    public class ToolItemSwitcher : MonoBehaviour
    {
        [SerializeField]
        private ToolItemObject[] toolHolders;

        private ToolItemObject _currentObject;
        public ToolItemObject currentObject => _currentObject;

        public void EnableCurrentToolParticle(bool p_enable)
        {
            if (_currentObject != null)
                _currentObject.EnableParticle(p_enable);
        }

        public void EnableTool(int index)
        {
            for (int i = 0; i < toolHolders.Length; i++)
            {
                toolHolders[i].gameObject.SetActive(index == i);

                if (index == i)
                    _currentObject = toolHolders[i];
            }
        }

        public void HideAll()
        {
            _currentObject = null;

            for (int i = 0; i < toolHolders.Length; i++)
            {
                toolHolders[i].gameObject.SetActive(false);
            }

        }
    }
}