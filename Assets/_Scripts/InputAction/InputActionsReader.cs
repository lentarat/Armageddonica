using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InputActionsReader : IInputActionsReader, IInitializable, IDisposable
{
    private InputActions _inputActions;

    void IInitializable.Initialize()
    { 
        _inputActions = new InputActions();
        _inputActions.Enable();
    }

    void IDisposable.Dispose()
    {
        _inputActions?.Dispose();
    }

    Vector2 IInputActionsReader.GetMoveInputNormalized()
    {
        return _inputActions.Camera.WASD.ReadValue<Vector2>();
    }
}
