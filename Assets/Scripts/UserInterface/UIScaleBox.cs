using UnityEngine;

namespace UserInterface
{
    public class UIScaleBox : MonoBehaviour
    {
        public RectTransform targetElement;
        public Vector2 maxSize;

        void Update()
        {
            if (targetElement == null) return;

            Vector2 currentSize = targetElement.sizeDelta;
            float widthScale = maxSize.x / currentSize.x;
            float heightScale = maxSize.y / currentSize.y;
            float scale = Mathf.Min(widthScale, heightScale, 1f); // Choose the smaller scale factor

            targetElement.localScale = new Vector3(scale, scale, 1f);
        }
    }
}