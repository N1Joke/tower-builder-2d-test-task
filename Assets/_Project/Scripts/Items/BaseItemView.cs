using Assets._Project.Scripts.Tools;
using UnityEngine;

namespace Assets._Project.Scripts.Items
{
    public class BaseItemView : MonoBehaviour
    {
        [field: SerializeField] public Collider2D collider2d { get; private set; }
        [field: SerializeField] public SpriteRenderer spriteRenderer { get; private set; }
        [field: SerializeField] public SceneDragHandler dragHandler { get; private set; }
    }
}
