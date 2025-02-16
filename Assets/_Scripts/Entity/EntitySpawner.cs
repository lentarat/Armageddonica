using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] private EntityType _pointerSpawnedEntityType;
    [SerializeField] private Entity _blueEntityPrefab;
    [SerializeField] private Entity _redEntityPrefab;
    [SerializeField] private Entity _pinkEntityPrefab;

    private IEntitySpawnPositionRevealable _entitySpawnPositionRevealable;

    [Inject]
    private void Construct(IEntitySpawnPositionRevealable entitySpawnPositionRevealable)
    { 
        _entitySpawnPositionRevealable = entitySpawnPositionRevealable;
        Subscribe();
    }

    private void Subscribe()
    {
        _entitySpawnPositionRevealable.OnEntitySpawnPositionRevealed += SpawnEntity;
    }

    private void SpawnEntity(Vector3 spawnPosition)
    {
        Instantiate(_blueEntityPrefab, spawnPosition, Quaternion.identity);
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void Unsubscribe()
    {
        _entitySpawnPositionRevealable.OnEntitySpawnPositionRevealed -= SpawnEntity;
    }
}
