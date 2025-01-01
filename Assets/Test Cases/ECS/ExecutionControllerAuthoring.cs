using Unity.Entities;
using UnityEngine;



class ExecutionControllerAuthoring : MonoBehaviour {

    [Header("Tags for the Sub Scene")]
    public bool ECS;
    public bool ECSComputeShader;
    public bool ECSPhysics;
    
}



class ExecutionControllerAuthoringBaker : Baker<ExecutionControllerAuthoring> {



    public override void Bake(ExecutionControllerAuthoring authoring) {
        var entity = GetEntity(TransformUsageFlags.None);

        if (authoring.ECS)
            AddComponent<ECSTag>(entity);

        if (authoring.ECSComputeShader)
            AddComponent<ECSComputeShaderTag>(entity);

        if (authoring.ECSPhysics)
            AddComponent<ECSPhysicsTag>(entity);

    }



}



public struct ECSTag : IComponentData {

}



public struct ECSComputeShaderTag : IComponentData {

}



public struct ECSPhysicsTag : IComponentData {

}
