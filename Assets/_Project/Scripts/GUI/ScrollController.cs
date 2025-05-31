using Assets._Project.Scripts.Gameplay;
using Assets._Project.Scripts.Items;
using Assets._Project.Scripts.Presets;
using Core;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Scripts.GUI
{
    public class ScrollController : BaseDisposable, IScrollChecker
    {
        public struct Ctx
        {
            public ItemConfig config;
            public ScrollRect scrollRect;
            public RectTransform content;
            public Canvas canvas;
            public ITowerBuilder towerBuilder;
        }

        private readonly Ctx _ctx;
        private RectTransform _scrollRect;

        public ScrollController(Ctx ctx)
        {
            _ctx = ctx;

            Init();
        }

        private void Init()
        {
            _scrollRect = _ctx.scrollRect.GetComponent<RectTransform>();

            for (int i = 0; i < _ctx.config.itemSprites.Length; i++)
            {
                AddDispose(new BaseScrollItem(new BaseScrollItem.Ctx
                {
                    canvas = _ctx.canvas,
                    config = _ctx.config,
                    content = _ctx.content,
                    id = i,
                    scrollRect = _ctx.scrollRect,
                    towerBuilder = _ctx.towerBuilder,
                    scrollChecker = this
                }));
            }
        }

        public bool IsInScrollPlate(RectTransform rect)
        {
            Vector3[] scrollCorners = new Vector3[4];
            Vector3[] imageCorners = new Vector3[4];

            _scrollRect.GetWorldCorners(scrollCorners);
            rect.GetWorldCorners(imageCorners);

            Rect scrollWorldRect = new Rect(
                scrollCorners[0].x,
                scrollCorners[0].y,
                scrollCorners[2].x - scrollCorners[0].x,
                scrollCorners[2].y - scrollCorners[0].y
            );

            for (int i = 0; i < 4; i++)
            {
                if (scrollWorldRect.Contains(imageCorners[i]))
                {
                    Debug.Log("Corner  in plate");
                    return true; 
                }
            }

            Debug.Log("Corner not in plate");
            return false;
        }
    }
}
