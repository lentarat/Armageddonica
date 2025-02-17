using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EntityFactory
{
    private static int _counter;
    private ObjectPool<Entity> _entitiesPool;

    public EntityFactory(Entity entityPrefab, int initialSize, Transform parent, EntitySpawner entitySpawner, List<Entity> entitiesList)
    {
        Func<Entity> factoryMethod = () =>
        {
            Entity entity = UnityEngine.Object.Instantiate(entityPrefab, parent, false);
            entity.name = entity.name + ++_counter;
            entity.Initialize(entitySpawner);
            entitiesList.Add(entity);
            return entity;
        };
        Action<Entity> onTakeFromPool = (entity) => { entitiesList.Add(entity); };
        _entitiesPool = new ObjectPool<Entity>(factoryMethod, onTakeFromPool, initialSize);
    }

    public Entity Create(Vector3 position)
    {
        Entity entity = _entitiesPool.Get();
        entity.transform.position = position;
        entity.gameObject.SetActive(true);

        return entity;
    }

    public void Release(Entity entity)
    {
        _entitiesPool.Release(entity);
    }
}