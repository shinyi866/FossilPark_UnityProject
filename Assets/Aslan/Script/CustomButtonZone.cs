using UnityEngine;
using UnityEngine.UI;

public class CustomButtonZone : MonoBehaviour
{
    private void Awake()
    {
        Image image = this.GetComponent<Image>();
        image.alphaHitTestMinimumThreshold = 0.1f;
    }
}
