using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class InputActionsReader : IInputActionsReader, IInitializable, IDisposable
{
    private InputActions _inputActions;
    private event Action<Vector2> _onPointerDownPosition;

    void IInitializable.Initialize()
    { 
        _inputActions = new InputActions();
        _inputActions.Enable();
        Subscribe();
    }

    void IDisposable.Dispose()
    {
        _inputActions?.Dispose();
        Unsubscribe();
    }

    private void Subscribe()
    {
        _inputActions.Camera.MouseLeftClick.performed += HandleMouseLeftClicked;
    }

    private void HandleMouseLeftClicked(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = Pointer.current.position.ReadValue();
        _onPointerDownPosition?.Invoke(mousePosition);
    }

    private void Unsubscribe()
    {
        _inputActions.Camera.MouseLeftClick.performed -= HandleMouseLeftClicked;
    }


    event Action<Vector2> IInputActionsReader.OnPointerDownPosition
    {
        add
        {
            _onPointerDownPosition += value;
        }

        remove
        {
            _onPointerDownPosition -= value;
        }
    }

    Vector2 IInputActionsReader.GetMoveInputNormalized()
    {
        return _inputActions.Camera.WASD.ReadValue<Vector2>();
    }
}
