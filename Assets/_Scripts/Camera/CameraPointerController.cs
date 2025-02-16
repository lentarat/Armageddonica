using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class CameraPointerController : MonoBehaviour, IEntitySpawnPositionRevealable
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _levelLayerMask;

    private IInputActionsReader _inputActionsReader;
    private event Action<Vector3> _onEntitySpawnPositionRevealed;

    [Inject]
    private void Construct(IInputActionsReader inputActionsReader)
    {
        _inputActionsReader = inputActionsReader;
        Subscribe();
    }

    private void Subscribe()
    {
        _inputActionsReader.OnPointerDownPosition += HandlePointerDownPosition;
    }

    private void HandlePointerDownPosition(Vector2 screenPosition)
    {
        Ray ray = _camera.ScreenPointToRay(screenPosition);
        Physics.Raycast(ray.origin, ray.direction, out RaycastHit raycastHit, 10000f, _levelLayerMask.value);
        NavMesh.SamplePosition(raycastHit.point, out NavMeshHit navMeshHit, 10000f, NavMesh.AllAreas);
        Vector3 worldSpacePointerDownPosition = navMeshHit.position;

        _onEntitySpawnPositionRevealed?.Invoke(worldSpacePointerDownPosition);
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void Unsubscribe()
    {
        _inputActionsReader.OnPointerDownPosition -= HandlePointerDownPosition;
    }

    event Action<Vector3> IEntitySpawnPositionRevealable.OnEntitySpawnPositionRevealed
    {
        add
        {
            _onEntitySpawnPositionRevealed += value;
        }

        remove
        {
            _onEntitySpawnPositionRevealed -= value;
        }
    }
}
