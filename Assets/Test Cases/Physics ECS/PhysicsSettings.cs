using Unity.Entities;
using Unity.Mathematics;



public struct PhysicsSettings : IComponentData {

    public float3 TargetPosition;
    public float JumpForce;
    
}
