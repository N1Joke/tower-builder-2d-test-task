using Assets._Project.Scripts.Items;
using UnityEngine;

namespace Assets._Project.Scripts.Presets
{
    [CreateAssetMenu(fileName = "ItemConfig", menuName = "ScriptableObjects/ItemConfig", order = 1)]
    public class ItemConfig : ScriptableObject
    {
        [field: SerializeField] public BaseItemView itemView { get; private set; }
        [field: SerializeField] public ScrollItemView scrollItemViewPrefab { get; private set; }
        [field: SerializeField] public Sprite[] itemSprites { get; private set; }
    }
}
