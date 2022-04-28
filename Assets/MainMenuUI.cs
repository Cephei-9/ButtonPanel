using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private ButtonPanel _buttonPanel;
    [SerializeField] private MenuScrole _menuScrole;
    [Space]
    [SerializeField] private HorizontalSwipeInput _sensorInput;
    
    void Start()
    {
        Vector2 refResolution = GetComponentInParent<CanvasScaler>().referenceResolution;

        _menuScrole.Init(refResolution, _sensorInput);

        _buttonPanel.ActivateNewItemEvent += (x) => _menuScrole.SetNewActiveImages(x.Index);
        _buttonPanel.Init(refResolution, _sensorInput);

        _menuScrole.TeleportToTarget();
    }

    void Update()
    {
        _menuScrole.MoveToTarget();
    }

    [ContextMenu("SetMenuSprits")]
    public void SetMenuSprits()   
    {
        // Метод преднозначен для вызыва из инспектора каждый раз когда поменяется порядок PanelItem в массиве
        _menuScrole.SetSprits(_buttonPanel.Items.Select(x => x.IconSprite).ToArray());
    }
}
