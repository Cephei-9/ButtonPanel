using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanelItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private RectTransform _selfTransform;
    [SerializeField] private RectTransform _iconTransform;
    [Space]
    [SerializeField] private Text _text;
    [SerializeField] private Image _backgraundImage;

    public Sprite IconSprite => _defaultIconSprite;
    public int Index { get; set; }
    public Action<PanelItem> ClickEvent;

    [HideInInspector] 
    [SerializeField] 
    private Sprite _defaultBackgroundSprite;
    [HideInInspector]
    [SerializeField]
    private Sprite _defaultIconSprite;

    public void Active(PanelItemsSetup setup)
    {
        SetWidth(setup.WidthSelectedItem);

        _iconTransform.anchoredPosition += Vector2.up * setup.HightLiftIcon;
        _iconTransform.sizeDelta *= setup.ScaleIcon;

        _backgraundImage.sprite = setup.SelectedBackGraundSprite;
        _text.enabled = true;
    }

    public void Deactive(PanelItemsSetup setup)
    {
        SetWidth(setup.WidthUnselectedItem);

        _iconTransform.anchoredPosition -= Vector2.up * setup.HightLiftIcon;
        _iconTransform.sizeDelta /= setup.ScaleIcon;

        _backgraundImage.sprite = _defaultBackgroundSprite;
        _text.enabled = false;
    }

    [ContextMenu("SetDefaultSprite")]
    public void SetDefaultSprite()
    {
        // Подразумевается что метод будет вызываться один раз из инспектора, и кешировать спрайты, чтобы не делать этого в рантайме
        _defaultBackgroundSprite = _backgraundImage.sprite;
        _defaultIconSprite = _iconTransform.GetComponent<Image>().sprite;
    }

    public void SetPosition(float positionByX)
    {
        _selfTransform.anchoredPosition = Vector2.right * positionByX;
    }

    public void SetWidth(float width)
    {
        _selfTransform.sizeDelta = new Vector2(width, _selfTransform.sizeDelta.y);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ClickEvent.Invoke(this);
    }
}