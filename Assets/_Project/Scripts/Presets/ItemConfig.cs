using Assets._Project.Scripts.Items;
using System;
using UnityEngine;

namespace Assets._Project.Scripts.Presets
{
    [CreateAssetMenu(fileName = "ItemConfig", menuName = "ScriptableObjects/ItemConfig", order = 1)]
    public class ItemConfig : ScriptableObject
    {
        [field: SerializeField] public BaseItemView itemView { get; private set; }
        [field: SerializeField] public ScrollItemView scrollItemViewPrefab { get; private set; }
        [field: SerializeField] public Sprite[] itemSprites { get; private set; }

        public Sprite GetSprite(int index)
        {
            index = Mathf.Clamp(index, 0, itemSprites.Length - 1);
            return itemSprites[index];
        }
    }
}
