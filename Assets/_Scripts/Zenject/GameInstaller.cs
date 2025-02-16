using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<InputActionsReader>().AsSingle();
        Container.Bind<IEntitySpawnPositionRevealable>().To<CameraPointerController>().FromComponentInHierarchy().AsSingle();
    }
}
