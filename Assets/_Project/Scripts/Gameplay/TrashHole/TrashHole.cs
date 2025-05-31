using UnityEngine;
using Core;

namespace Assets._Project.Scripts.Gameplay
{
    public class TrashHole : BaseDisposable, ITrashHole
    {
        public struct Ctx
        {
            public TrashHoleView view;
        }

        private readonly Ctx _ctx;

        public TrashHole(Ctx ctx)
        {
            _ctx = ctx;
        }

        public SpriteRenderer hole;

        public Vector3 HoleCenter => _ctx.view.transform.position;

        public bool Utilize(SpriteRenderer cube)
        {
            Bounds bounds = cube.bounds;

            Vector2[] pointsToCheck = new Vector2[]
            {
                new Vector2(bounds.min.x, bounds.min.y),
                new Vector2(bounds.min.x, bounds.max.y),
                new Vector2(bounds.max.x, bounds.min.y),
                new Vector2(bounds.max.x, bounds.max.y)
            };

            for (int i = 0; i < pointsToCheck.Length; i++)
            {
                if (!_ctx.view.polygonCollider2D.OverlapPoint(pointsToCheck[i]))
                {
                    Debug.Log("Not in trash hole");
                    return false;
                }
            }

            Debug.Log("In trash hole");
            return true;
        }
    }
}
