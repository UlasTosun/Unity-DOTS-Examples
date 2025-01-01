using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Physics;
using Unity.Physics.Systems;



[CreateAfter(typeof(SpawnerSystem))]
[UpdateInGroup(typeof(PhysicsSystemGroup))]
[RequireMatchingQueriesForUpdate]
[BurstCompile]
partial struct PhysicsSystem : ISystem {



    [BurstCompile]
    public void OnCreate(ref SystemState state) {
        state.RequireForUpdate<ECSPhysicsTag>();
        state.RequireForUpdate<PhysicsSettings>();
        state.RequireForUpdate<PhysicsVelocity>();
    }



    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        PhysicsSettings physicsSettings = SystemAPI.GetSingleton<PhysicsSettings>();

        PhysicsJob physicsJob = new() {
            PhysicsSettings = physicsSettings
        };

        physicsJob.ScheduleParallel(); // No need to specify the query for IJobEntity, it will automatically create the query.

        state.Enabled = false;
    }



    [BurstCompile]
    public void OnDestroy(ref SystemState state) {
        
    }



}



[BurstCompile]
public partial struct PhysicsJob : IJobEntity {

    [ReadOnly] public PhysicsSettings PhysicsSettings;



    [BurstCompile]
    // IJobEntity will automatically create the query of LocalTransform for us.
    public void Execute(ref PhysicsVelocity physicsVelocity, in LocalToWorld localToWorld) {
        float jumpForce = PhysicsSettings.JumpForce;
        float3 direction = math.normalize(PhysicsSettings.TargetPosition - localToWorld.Position);
        physicsVelocity.Linear += direction * jumpForce;
    }



}
