using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MenuScrole
{
    [SerializeField] private float _speedMove = 5;
    [SerializeField] private float _maxOffset;
    [Space]
    [SerializeField] private RectTransform _menuTransform;
    [Space]
    [SerializeField] private Image[] _images;

    private float _refWidth;

    private Vector2 _fromItemPosition;
    private Vector2 _targetPosition;

    [SerializeField] private ISensorInput _input;

    public void Init(Vector2 referenceResolution, ISensorInput input)
    {
        _refWidth = referenceResolution.x;

        _input = input;
        _input.ChangeValueEvent += OnInputChangeValue;
        _input.PointerUpEvent += OnInputPointerUp;
    }

    public void MoveToTarget()
    {
        _menuTransform.anchoredPosition = Vector2.Lerp(_menuTransform.anchoredPosition, _targetPosition, _speedMove * Time.deltaTime);
    }

    public void SetSprits(Sprite[] sprits)
    {
        for (int i = 0; i < sprits.Length; i++)
            _images[i].sprite = sprits[i];
    }

    public void TeleportToTarget()
    {
        _menuTransform.anchoredPosition = _targetPosition;
    }

    public void SetNewActiveImages(int index)
    {
        _fromItemPosition = -_refWidth * Vector2.right * index;
        _targetPosition = _fromItemPosition;
    }

    private void OnInputChangeValue(Vector2 value)
    {
        _targetPosition += _refWidth * Vector2.right * value.x;
        Vector2 offset = _targetPosition - _fromItemPosition;
        _targetPosition = _fromItemPosition + offset.normalized * Mathf.Min(offset.magnitude, _maxOffset);
    }

    private void OnInputPointerUp()
    {
        _targetPosition = _fromItemPosition;
    }
}
