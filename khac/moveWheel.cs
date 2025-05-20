/*using UnityEngine;
using UnityEngine.EventSystems;

public class moveWheel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 offset;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            canvas.worldCamera, // dung dung camera nay
            out Vector2 localPointerPosition
        );
        offset = rectTransform.anchoredPosition - localPointerPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera, // Dung dung camera nay
            out Vector2 localPoint))
        {
            Vector2 newPos = localPoint + offset;
            rectTransform.anchoredPosition = ClampToCanvas(newPos);
        }
    }

    public void OnEndDrag(PointerEventData eventData) { }

    private Vector2 ClampToCanvas(Vector2 targetPos)
    {
        RectTransform canvasRect = canvas.transform as RectTransform;
        Vector2 halfSize = rectTransform.rect.size * 0.5f;
        Vector2 minPos = canvasRect.rect.min + halfSize;
        Vector2 maxPos = canvasRect.rect.max - halfSize;
        return new Vector2(
            Mathf.Clamp(targetPos.x, minPos.x, maxPos.x),
            Mathf.Clamp(targetPos.y, minPos.y, maxPos.y)
        );
    }
}*/