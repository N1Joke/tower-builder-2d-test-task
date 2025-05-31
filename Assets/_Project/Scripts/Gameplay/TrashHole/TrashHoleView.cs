using UnityEngine;

namespace Assets._Project.Scripts.Gameplay
{
    public class TrashHoleView : MonoBehaviour
    {
        [field: SerializeField] public PolygonCollider2D polygonCollider2D { get; private set; }
    }
}