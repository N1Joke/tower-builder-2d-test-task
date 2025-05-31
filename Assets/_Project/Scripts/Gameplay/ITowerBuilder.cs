using Assets._Project.Scripts.Items;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay
{
    public interface ITowerBuilder
    {
        public bool PlaceItem(RectTransform rect, int id);
        public bool IsOverValidBorder(Vector2 screenPosition, RectTransform rectTransform);
    }
}
