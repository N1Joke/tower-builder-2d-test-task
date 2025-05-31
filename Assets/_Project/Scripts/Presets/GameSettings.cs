using System;
using UnityEngine;

namespace Presets
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]
    public class GameSettings : ScriptableObject
    {
        [field: SerializeField] public float delayBetweenRemoveElementFromTower = 0.1f;
        [field: SerializeField] public float cubeJumpPower = 3f;
        [field: SerializeField] public float cubeJumpTime = 0.35f;
        [field: SerializeField] public float cubeDestroyTime = 0.2f;
    }
}