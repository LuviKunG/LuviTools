using UnityEngine;

public static class RectTransformExtension
{
    public static void SetAnchoredPosition(this RectTransform rectTransform, Setter<Vector2> setter)
    {
        rectTransform.anchoredPosition = setter(rectTransform.anchoredPosition);
    }
}
