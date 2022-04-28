using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class ButtonPanel
{
    [SerializeField] private PanelItemsSetup _itemsSetup;
    [Space]
    [SerializeField] private PanelItem _startActiveItem;
    [Space]
    [SerializeField] private PanelItem[] _items;
    public IEnumerable<PanelItem> Items => _items;

    public event Action<PanelItem> ActivateNewItemEvent;

    private float _refWidth;
    private float _widthUnselectedItem;

    private ISensorInput _input;
    private PanelItem _activeItem;

    public void Init(Vector2 referenceResolution, ISensorInput input)
    {
        _input = input;
        _input.SwipeEvent += OnSwipe;

        _refWidth = referenceResolution.x;
        _widthUnselectedItem = (_refWidth - _itemsSetup.WidthSelectedItem) / (_items.Length - 1);
        _itemsSetup.WidthUnselectedItem = _widthUnselectedItem;

        for (int i = 0; i < _items.Length; i++)
        {
            _items[i].SetWidth(_widthUnselectedItem);

            _items[i].Index = i;
            _items[i].ClickEvent += ActivateItem;
        }

        ActivateItem(_startActiveItem);
    }

    private void OnSwipe(Vector2Int sign)
    {
        PanelItem nextItem = _items.FirstOrDefault(
            x => x.Index == _activeItem.Index - sign.x);
        if (nextItem)
            ActivateItem(nextItem);
    }

    private void ActivateItem(PanelItem itemToActive)
    {
        if (itemToActive == _activeItem) return;
        _activeItem?.Deactive(_itemsSetup);

        PanelItem[] itemsBefore = _items.Where(x => x.Index < itemToActive.Index).ToArray();
        LineUpItems(0, itemsBefore);

        PanelItem[] itemsAfter = _items.Where(x => x.Index > itemToActive.Index).ToArray();
        float startPositionForItemsAfter = _refWidth - (_widthUnselectedItem * itemsAfter.Length);
        LineUpItems(startPositionForItemsAfter, itemsAfter);

        float positionForItemAfter = startPositionForItemsAfter - _itemsSetup.WidthSelectedItem;
        itemToActive.SetPosition(positionForItemAfter);

        itemToActive.Active(_itemsSetup);
        _activeItem = itemToActive;

        ActivateNewItemEvent?.Invoke(_activeItem);
    }

    private void LineUpItems(float startPositionByX, params PanelItem[] items)
    {
        if (items.Length == 0) return;

        for (int i = 0; i < items.Length; i++)
        {
            float position = startPositionByX + (_widthUnselectedItem * i);
            items[i].SetPosition(position);
        }
    }
}

[Serializable]
public struct PanelItemsSetup
{
    public float HightLiftIcon;

    public float WidthSelectedItem;
    [HideInInspector] public float WidthUnselectedItem;

    public float ScaleIcon;
    public Sprite SelectedBackGraundSprite;
}

