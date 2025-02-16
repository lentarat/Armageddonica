using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IInputActionsReader
{
    event Action<Vector2> OnPointerDownPosition;
    Vector2 GetMoveInputNormalized();
}
