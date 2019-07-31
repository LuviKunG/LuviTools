public delegate T Setter<T>(T getter);

// Usage Example.
//public static void SetAnchoredPosition(this RectTransform rectTransform, Setter<Vector2> setter)
//{
//    rectTransform.anchoredPosition = setter(rectTransform.anchoredPosition);
//}