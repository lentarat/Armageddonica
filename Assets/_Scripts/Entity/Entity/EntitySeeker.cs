using Cysharp.Threading.Tasks;
using ProjectDawn.Navigation.Hybrid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditorInternal;
using UnityEngine;
using Zenject;

public class EntitySeeker : MonoBehaviour
{
    [SerializeField] private Entity _searcherEntity;
    [SerializeField, Tooltip("Milliseconds")] private int _lookForNewClosestEntityPeriod;

    public Transform FollowedEntityTransform { get; private set; }

    private bool _isSeeking;
    private CancellationTokenSource _cancellationTokenSource;
    private List<Entity> _entitiesList;
    private EntityType _seekerEntityType;

    private void Start()
    {
        _entitiesList = _searcherEntity.EntitiesList;
        _seekerEntityType = _searcherEntity.EntityType;
        StartSeekingTheClosestEnemyEntity();
    }

    private void StartSeekingTheClosestEnemyEntity()
    {
        if (_isSeeking)
        {
            return;
        }

        _cancellationTokenSource = new CancellationTokenSource();
        _isSeeking = true;
        KeepFindingTheClosestEntity(_cancellationTokenSource).Forget();
    }

    private async UniTaskVoid KeepFindingTheClosestEntity(CancellationTokenSource cancellationTokenSource)
    {
        try
        {
            while (cancellationTokenSource.IsCancellationRequested == false)
            {
                if (_entitiesList.Count == 0)
                {
                    await UniTask.Delay(_lookForNewClosestEntityPeriod);
                }

                float closestSqrMagnitude = float.MaxValue;
                int closestEntityIndex = 0;

                for (int i = 0; i < _entitiesList.Count; i++)
                {
                    if (_entitiesList[i].EntityType == _searcherEntity.EntityType)
                    {
                        continue;
                    }

                    float distanceSquared = GetDistanceSquared(transform.position, _entitiesList[i].transform.position);
                    if (distanceSquared < closestSqrMagnitude)
                    {
                        closestSqrMagnitude = distanceSquared;
                        closestEntityIndex = i;
                    }
                }
                FollowedEntityTransform = _entitiesList[closestEntityIndex].transform;

                await UniTask.Delay(_lookForNewClosestEntityPeriod);
            }
        }
        catch (OperationCanceledException e)
        {
            Debug.Log(e.Message);
        }
        finally
        {
            _isSeeking = false;
        }
    }

    private float GetDistanceSquared(Vector3 a, Vector3 b)
    {
        return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y) + (a.z - b.z) * (a.z - b.z);
    }

    private void StopSeeking()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _isSeeking = false;
    }
}
