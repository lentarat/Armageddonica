using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _velocityMultiplier;
    [SerializeField] private float _acceleration;

    private Vector3 _currentVelocity;
    private IInputActionsReader _inputActionsReader;
    
    [Inject]
    private void Construct(IInputActionsReader inputActionsReader)
    { 
        _inputActionsReader = inputActionsReader;
    }

    private void Update()
    {
        ProcessMovement();
    }

    private void ProcessMovement()
    {
        Vector2 moveInputNormalized = _inputActionsReader.GetMoveInputNormalized();
        if (moveInputNormalized != Vector2.zero || _currentVelocity != Vector3.zero)
        {
            Vector3 moveDirection = new Vector3(moveInputNormalized.x, 0f, moveInputNormalized.y);
            Move(moveDirection);
        }
    }

    private void Move(Vector3 moveDirection)
    {
        Vector3 forwardVector = transform.forward;
        forwardVector.y = 0f;
        forwardVector.Normalize();
        Vector3 moveVectorNormalized = transform.right * moveDirection.x + forwardVector * moveDirection.z;

        _currentVelocity = Vector3.Lerp(_currentVelocity, moveVectorNormalized * _velocityMultiplier, Time.deltaTime * _acceleration);
        
        transform.position += _currentVelocity * Time.deltaTime;
    }
}
