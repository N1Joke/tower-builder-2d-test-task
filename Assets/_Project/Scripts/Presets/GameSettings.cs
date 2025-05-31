using UnityEngine;

namespace Presets
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]
    public class GameSettings : ScriptableObject
    {
        [field: SerializeField] public float loadingImageRotationSpeed { get; private set; } = 10;
    }
}