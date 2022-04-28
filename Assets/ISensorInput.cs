using System;
using UnityEngine;

public interface ISensorInput
{
    event Action PointerDownEvent;
    event Action PointerUpEvent;
    event Action<Vector2> ChangeValueEvent;
    event Action<Vector2Int> SwipeEvent;

    Vector2 CurentDelta { get; }
}