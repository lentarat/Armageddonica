using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;
using System.Linq;
using Cysharp.Threading.Tasks;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] private Entity _blueEntityPrefab;
    [SerializeField] private Entity _redEntityPrefab;
    [SerializeField] private Entity _pinkEntityPrefab;
    [SerializeField] private EntityType _pointerSpawnedEntityType;
    [SerializeField] private EntityTypeSpawnData[] _entityTypeInitialSpawnData;

    private List<Entity> _entitiesList = new();
    public List<Entity> EntitiesList => _entitiesList;

    private float _cleanEnemiesListPeriod = 5f;
    private Dictionary<EntityType, EntityFactory> _entityTypesToFactories;
    private IEntitySpawnPositionRevealable _entitySpawnPositionRevealable;

    [Serializable]
    private struct EntityTypeSpawnData
    {
        [SerializeField] private EntityType _entityType;
        public EntityType EntityType => _entityType;
        [SerializeField] private int _count;
        public int Count => _count;
        [SerializeField] private Transform _parent;
        public Transform Parent => _parent;
    }

    public void ReleaseEntity(Entity entity)
    {
        _entityTypesToFactories[entity.EntityType].Release(entity);
        Debug.Log("Entity Released, Count: " + _entitiesList.Count);
    }

    [Inject]
    private void Construct(IEntitySpawnPositionRevealable entitySpawnPositionRevealable)
    { 
        _entitySpawnPositionRevealable = entitySpawnPositionRevealable;
        Subscribe();
        CreateFactories();
        StartCleaningEnemiesList().Forget();
    }

    private void Subscribe()
    {
        _entitySpawnPositionRevealable.OnEntitySpawnPositionRevealed += SpawnEntity;
    }

    private void SpawnEntity(Vector3 spawnPosition)
    {
        _entityTypesToFactories[_pointerSpawnedEntityType].Create(spawnPosition);
        Debug.Log("Entity Added, Count: " + _entitiesList.Count);
    }

    private void CreateFactories()
    {
        _entityTypesToFactories = new Dictionary<EntityType, EntityFactory>()
        {
            { EntityType.Blue, new EntityFactory(_blueEntityPrefab, GetCount(EntityType.Blue), GetParent(EntityType.Blue), this, _entitiesList)},
            { EntityType.Red, new EntityFactory(_redEntityPrefab, GetCount(EntityType.Red), GetParent(EntityType.Red), this, _entitiesList)},
            { EntityType.Pink, new EntityFactory(_pinkEntityPrefab, GetCount(EntityType.Pink), GetParent(EntityType.Pink), this, _entitiesList)}
        };

        int GetCount(EntityType entityType)
        { 
            int count = _entityTypeInitialSpawnData
            .Where(x => x.EntityType == entityType)
            .Select(x => x.Count)
            .FirstOrDefault();

            return count;
        }

        Transform GetParent(EntityType entityType) 
        {
            Transform parent = _entityTypeInitialSpawnData
                .Where(x => x.EntityType == entityType)
                .Select(x => x.Parent)
                .FirstOrDefault();

            return parent;
        }
    }

    private async UniTaskVoid StartCleaningEnemiesList()
    { 
        while(true)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_cleanEnemiesListPeriod));
            int beforeCleaning = _entitiesList.Count;
            _entitiesList.RemoveAll(x => x == null || x.gameObject.activeInHierarchy == false);
            int afterCleaning = _entitiesList.Count;
            Debug.Log($"Cleared {beforeCleaning - afterCleaning} entitites");
        }
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
