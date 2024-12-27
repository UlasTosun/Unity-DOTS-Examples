using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Physics;
using Unity.Physics.Systems;



[UpdateInGroup(typeof(PhysicsSystemGroup))]
[RequireMatchingQueriesForUpdate]
[BurstCompile]
partial struct PhysicsSystem : ISystem {



    [BurstCompile]
    public void OnCreate(ref SystemState state) {
        state.RequireForUpdate<PhysicsSettings>();
        state.RequireForUpdate<PhysicsVelocity>();
    }



    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        PhysicsSettings physicsSettings = SystemAPI.GetSingleton<PhysicsSettings>();
        EntityQuery query = SystemAPI.QueryBuilder().WithAll<PhysicsVelocity, LocalToWorld>().Build();

        PhysicsJob physicsJob = new() {
            PhysicsSettings = physicsSettings
        };

        physicsJob.ScheduleParallel(query);

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
    public void Execute(ref PhysicsVelocity physicsVelocity, in LocalToWorld localToWorld) {
        float jumpForce = PhysicsSettings.JumpForce;
        float3 direction = math.normalize(PhysicsSettings.TargetPosition - localToWorld.Position);
        physicsVelocity.Linear += direction * jumpForce;
    }



}
