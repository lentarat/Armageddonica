using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private LayerMask _entityLayerMask;
    [SerializeField] private EntityType _entityType;
    public EntityType EntityType => _entityType;

    private EntitySpawner _entitySpawner;
    public List<Entity> EntitiesList => _entitySpawner.EntitiesList;

    public void Initialize(EntitySpawner entitySpawner)
    {
        _entitySpawner ??= entitySpawner;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == _entityLayerMask)
        {
            Entity entity = collision.gameObject.GetComponent<Entity>();
            if(entity.EntityType != _entityType)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        _entitySpawner.ReleaseEntity(this);
    }
}
