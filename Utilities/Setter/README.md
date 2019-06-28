# Setter
It's delegate that able to get the property as field and modifiable, then set back to it's property.

## Example

### RectTransformExtension
```C#
public static class RectTransformExtension
{
	public static void SetAnchoredPosition(this RectTransform rectTransform, Setter<Vector2> setter)
	{
		rectTransform.anchoredPosition = setter(rectTransform.anchoredPosition);
	}
}
```