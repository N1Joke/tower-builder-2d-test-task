using Assets._Project.Scripts.Items;
using UnityEngine;
using Core;
using UnityEngine.UI;

namespace Assets._Project.Scripts.Gameplay
{
    public class TowerBuilder : BaseDisposable, ITowerBuilder
    {
        public struct Ctx
        {
            public SpriteRenderer spriteRenderer;
            public Camera camera;
        }

        private const int CubeSortOrder = 1;
        private const float CubeSize = 0.8f;
        protected readonly Ctx _ctx;
        protected ScrollItemView _view;
               
        public TowerBuilder(Ctx ctx)
        {
            _ctx = ctx;
        }

        public bool PlaceItem(RectTransform rect, int id)
        {
            if (IsOverValidBorder(rect.anchoredPosition, rect))
            {
                SpriteRenderer spawned = SpawnSpriteFromImageOnCamera(rect.GetComponent<Image>(), _ctx.camera);
                Debug.Log($"[TowerBuilder] valid pos {rect.anchoredPosition}");
                return true;
            }

            Debug.Log($"[TowerBuilder] invalid pos {rect.anchoredPosition}");
            return false;
        }

        public bool IsOverValidBorder(Vector2 screenPosition, RectTransform rectTransform)
        {
            // 1. Получаем углы UI элемента
            Vector3[] imageCorners = new Vector3[4];
            rectTransform.GetWorldCorners(imageCorners);

            // Переводим углы UI в экранные координаты
            for (int i = 0; i < 4; i++)
                imageCorners[i] = RectTransformUtility.WorldToScreenPoint(null, imageCorners[i]);

            // Формируем прямоугольник UI-элемента
            Rect imageRect = new Rect(
                imageCorners[0].x,
                imageCorners[0].y,
                imageCorners[2].x - imageCorners[0].x,
                imageCorners[2].y - imageCorners[0].y
            );

            // 2. Получаем границы SpriteRenderer в экранных координатах
            Bounds bounds = _ctx.spriteRenderer.bounds;
            Vector3 min = _ctx.camera.WorldToScreenPoint(bounds.min);
            Vector3 max = _ctx.camera.WorldToScreenPoint(bounds.max);

            Rect spriteRect = new Rect(min.x, min.y, max.x - min.x, max.y - min.y);

            // 3. Проверка: полностью ли imageRect внутри spriteRect
            bool isFullyInsideSprite = spriteRect.Contains(imageRect.min) && spriteRect.Contains(imageRect.max);

            // 4. Проверка: полностью ли imageRect находится на экране
            Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
            bool isFullyInsideScreen = screenRect.Contains(imageRect.min) && screenRect.Contains(imageRect.max);

            return isFullyInsideSprite && isFullyInsideScreen;
        }                

        public static SpriteRenderer SpawnSpriteFromImageOnCamera(Image image, Camera camera, Transform parent = null)
        {
            RectTransform rectTransform = image.rectTransform;
            Sprite sprite = image.sprite;

            // Получаем размеры UI элемента на экране
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            Vector3 screenMin = RectTransformUtility.WorldToScreenPoint(null, corners[0]);
            Vector3 screenMax = RectTransformUtility.WorldToScreenPoint(null, corners[2]);

            Vector3 screenCenter = (screenMin + screenMax) / 2f;

            Vector3 worldPos = camera.ScreenToWorldPoint(new Vector3(screenCenter.x, screenCenter.y, 1f));

            GameObject go = new GameObject("Cube_" + sprite.name);
            if (parent != null)
                go.transform.SetParent(parent);

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.sortingOrder = CubeSortOrder;
            go.transform.position = worldPos;
            go.transform.localScale = new Vector3(CubeSize, CubeSize, CubeSize);

            return sr;
        }

    }
}