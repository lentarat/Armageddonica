using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IEntitySpawnPositionRevealable
{
    event Action<Vector3> OnEntitySpawnPositionRevealed;
}
