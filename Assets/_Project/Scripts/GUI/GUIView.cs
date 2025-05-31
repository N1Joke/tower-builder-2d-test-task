using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GUI
{
    public class GUIView : MonoBehaviour
    {
        [field: SerializeField] public ScrollRect scrollRect;
        [field: SerializeField] public RectTransform scrollContent;
        [field: SerializeField] public Canvas canvas;
        [field: SerializeField] public TextMeshProUGUI messageContainer;
    }
}