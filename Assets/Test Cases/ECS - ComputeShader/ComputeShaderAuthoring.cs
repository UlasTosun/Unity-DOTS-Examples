using Unity.Entities;
using UnityEngine;



public class ComputeShaderAuthoring : MonoBehaviour {

    [Header("References")]
    [Tooltip("The compute shader to animate with.")]
    public ComputeShader ComputeShader;

}



public class ComputeShaderAuthoringBaker : Baker<ComputeShaderAuthoring> {



    public override void Bake(ComputeShaderAuthoring authoring) {
        Entity entity = GetEntity(TransformUsageFlags.None);

        ComputeShaderData computeShaderData = new () {
            ComputeShader = authoring.ComputeShader
        };

        AddComponentObject(entity, computeShaderData); // Use AddComponentObject instead of AddComponent for managed components.
    }



}
