using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class HorizontalSwipeInput : MonoBehaviour, ISensorInput, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float _sensitivity = 1;
    [SerializeField] private float _deltaForSwipe = 0.2f;

    public event Action PointerDownEvent;
    public event Action PointerUpEvent;
    public event Action<Vector2> ChangeValueEvent;
    public event Action<Vector2Int> SwipeEvent;

    private float _curentDelta;
    private float _screenWidth;

    public Vector2 CurentDelta => Vector2.right * _curentDelta;

    private void Start()
    {
        _screenWidth = Screen.width;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PointerDownEvent?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PointerUpEvent?.Invoke();
        _curentDelta = 0;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 delta = eventData.delta * _sensitivity / _screenWidth;
        _curentDelta += delta.x;
        ChangeValueEvent?.Invoke(delta);

        if (Mathf.Abs(_curentDelta) >= _deltaForSwipe)
        {
            SwipeEvent?.Invoke(Vector2Int.right * (int)Mathf.Sign(_curentDelta));
            _curentDelta = 0;
        }
    }
}
