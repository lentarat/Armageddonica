using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityFactory 
{
    Entity Create(Vector3 position);
}
