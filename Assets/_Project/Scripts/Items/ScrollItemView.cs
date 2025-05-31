using Assets._Project.Scripts.Tools;
using UnityEngine;

namespace Assets._Project.Scripts.Items
{
    public class ScrollItemView : MonoBehaviour
    {
        [field: SerializeField] public DragHandler dragHandler { get; private set; }
    }
}