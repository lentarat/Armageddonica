using Cysharp.Threading.Tasks;
using ProjectDawn.Navigation.Hybrid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public class EntityMovement : MonoBehaviour
{
    [SerializeField] private EntitySeeker _entitySeeker;
    [SerializeField] private AgentAuthoring _agentAuthoring;
    [SerializeField, Tooltip("Milliseconds")] private int _changeFollowedEntityPeriod;
    
    private bool _isMoving;
    private CancellationTokenSource _cancellationTokenSource;

    private void Start()
    {
        KeepFollowingClosestEntity().Forget();
    }

    private async UniTaskVoid KeepFollowingClosestEntity()
    {
        if (_isMoving)
        {
            return;
        }

        _cancellationTokenSource = new CancellationTokenSource();
        _isMoving = true;

        try
        {
            while (_cancellationTokenSource.IsCancellationRequested == false)
            {
                if (_entitySeeker.FollowedEntityTransform != null)
                {
                    _agentAuthoring.SetDestinationDeferred(_entitySeeker.FollowedEntityTransform.position);
                }

                await UniTask.Delay(_changeFollowedEntityPeriod);
            }
        }
        catch (OperationCanceledException e)
        {
            Debug.Log(e.Message);
        }
        finally
        {
            _isMoving = false;
        }
    }

    private void StopMoving()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _isMoving = false;
    }
}
