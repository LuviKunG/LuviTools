# Setter
It's delegate that able to get the property as field and modifiable, then set back to it's property.

## Example

### RectTransformExtension
```C#
//Extension of Rect Transform

public static class RectTransformExtension
{
	public static void SetAnchoredPosition(this RectTransform rectTransform, Setter<Vector2> setter)
	{
		rectTransform.anchoredPosition = setter(rectTransform.anchoredPosition);
	}
}

//Extension Usage Example

public class TestUI : MonoBehaviour
{
	[SerializeField]
    private RectTransform rectTransformIcon = default;

	public void Show()
	{
		rectTransformIcon.SetAnchoredPosition(pos => { pos.y = 0.0f; return pos; });
	}
	
	public void Hide()
	{
		rectTransformIcon.SetAnchoredPosition(pos => { pos.y = 800.0f; return pos; });
	}
}
```