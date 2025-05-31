using UnityEngine;

namespace Assets._Project.Scripts.GUI
{
    public interface IScrollChecker
    {
        public bool IsInScrollPlate(RectTransform rectTransform);
    }
}