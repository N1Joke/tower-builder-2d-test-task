using UnityEngine;

namespace Assets._Project.Scripts.Gameplay
{
    public interface ITrashHole
    {
        public bool Utilize(SpriteRenderer cube);
        public Vector3 HoleCenter { get; }
    }
}
