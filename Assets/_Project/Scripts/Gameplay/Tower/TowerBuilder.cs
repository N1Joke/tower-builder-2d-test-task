using UnityEngine;
using Core;
using UnityEngine.UI;
using Assets._Project.Scripts.Items;
using System.Collections.Generic;
using Assets._Project.Scripts.Presets;
using UniRx;
using Cysharp.Threading.Tasks;

namespace Assets._Project.Scripts.Gameplay
{
    public class TowerBuilder : BaseDisposable, ITowerBuilder
    {
        public struct Ctx
        {
            public SpriteRenderer spriteRenderer;
            public Camera camera;
            public ITrashHole trashHole;
            public ItemConfig config;             
        }

        private const int CubeSortOrder = 1;
        private const float CubeSize = 0.8f;
        private const float CubeFace = 1.422f;
        private readonly Ctx _ctx;
        private List<BaseItem> _items = new List<BaseItem>(10);

        public TowerBuilder(Ctx ctx)
        {
            _ctx = ctx;
        }

        public bool PlaceItem(RectTransform rect, int id)
        {
            Vector3 worldPos = GetWorldPos(rect);

            if (IsOverValidPosition(rect, worldPos))
            {
                BaseItemView baseItemView = SpawnSpriteFromImageOnCamera(rect.GetComponent<Image>(), worldPos);
                var item = new BaseItem(new BaseItem.Ctx
                {
                    id = id,
                    view = baseItemView,
                    trashHole = _ctx.trashHole
                });

                PlaceCubeWithLogic(item);
                _items.Add(item);
                AddDispose(item.OnDestroy.SubscribeWithSkip(RemoveElementFromTower));
                Debug.Log($"[TowerBuilder] valid pos {rect.anchoredPosition}");
                return true;
            }

            Debug.Log($"[TowerBuilder] invalid pos {rect.anchoredPosition}");
            return false;
        }

        private Vector3 GetWorldPos(RectTransform rectTransform)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            Vector3 screenMin = RectTransformUtility.WorldToScreenPoint(null, corners[0]);
            Vector3 screenMax = RectTransformUtility.WorldToScreenPoint(null, corners[2]);

            Vector3 screenCenter = (screenMin + screenMax) / 2f;

            Vector3 worldPos = _ctx.camera.ScreenToWorldPoint(new Vector3(screenCenter.x, screenCenter.y, 1f));
            worldPos.z = 0f;

            return worldPos;
        }

        private void PlaceCubeWithLogic(BaseItem item)
        {
            if (_items.Count == 0)
                return;

            Vector3 posToJump = _items[_items.Count - 1].Pos;
            posToJump += new Vector3(Random.Range(-CubeFace / 2f, CubeFace / 2f), CubeFace, item.Pos.z);
            item.JumpIntoPos(posToJump);
        }

        private bool IsOverValidPosition(RectTransform rectTransform, Vector3 worldPos)
        {
            Vector3[] imageCorners = new Vector3[4];
            rectTransform.GetWorldCorners(imageCorners);

            for (int i = 0; i < 4; i++)
                imageCorners[i] = RectTransformUtility.WorldToScreenPoint(null, imageCorners[i]);

            Rect imageRect = new Rect(
                imageCorners[0].x,
                imageCorners[0].y,
                imageCorners[2].x - imageCorners[0].x,
                imageCorners[2].y - imageCorners[0].y
            );

            Bounds bounds = _ctx.spriteRenderer.bounds;
            Vector3 min = _ctx.camera.WorldToScreenPoint(bounds.min);
            Vector3 max = _ctx.camera.WorldToScreenPoint(bounds.max);

            Rect spriteRect = new Rect(min.x, min.y, max.x - min.x, max.y - min.y);

            bool isFullyInsideSprite = spriteRect.Contains(imageRect.min) && spriteRect.Contains(imageRect.max);

            Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
            bool isFullyInsideScreen = screenRect.Contains(imageRect.min) && screenRect.Contains(imageRect.max);

            return isFullyInsideSprite && isFullyInsideScreen && CanPlaceOnTower(worldPos);
        }

        private bool CanPlaceOnTower(Vector3 worldPos)
        {
            if (_items.Count == 0)
                return true;

            var lastItem = _items[_items.Count - 1];

            float halfLength = CubeFace / 2f;

            float currentLeft = worldPos.x - halfLength;
            float currentRight = worldPos.x + halfLength;

            float lastLeft = lastItem.Pos.x - halfLength;
            float lastRight = lastItem.Pos.x + halfLength;

            float overlapLeft = Mathf.Max(currentLeft, lastLeft);
            float overlapRight = Mathf.Min(currentRight, lastRight);
            float overlap = overlapRight - overlapLeft;

            return overlap >= (CubeFace / 2f) && worldPos.y >= lastItem.Pos.y + CubeFace;
        }

        private BaseItemView SpawnSpriteFromImageOnCamera(Image image, Vector3 worldPos)
        {
            BaseItemView itemView = GameObject.Instantiate(_ctx.config.itemView);

            itemView.spriteRenderer.sprite = image.sprite;
            itemView.spriteRenderer.sortingOrder = CubeSortOrder;
            itemView.transform.position = worldPos;
            itemView.transform.localScale = new Vector3(CubeSize, CubeSize, CubeSize);

            return itemView;
        }

        private async void RemoveElementFromTower(BaseItem baseItem)
        {
            int indexToRemove = _items.IndexOf(baseItem);
           
            _items.RemoveAt(indexToRemove);

            Vector3 currentPos = baseItem.TowerPos;
            for (int i = indexToRemove; i < _items.Count; i++)
            {
                Vector3 nextPos = _items[i].TowerPos;
                _items[i].MoveTo(currentPos);
                currentPos = nextPos;
                await UniTask.Delay(100);
            }
        }


        protected override void OnDispose()
        {
            for (int i = 0; i < _items.Count; i++)
                _items[i]?.Dispose();

            base.OnDispose();
        }
    }
}