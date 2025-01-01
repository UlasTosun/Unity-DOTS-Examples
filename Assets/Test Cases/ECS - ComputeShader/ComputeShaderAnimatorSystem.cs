using UnityEngine;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;



[CreateAfter(typeof(SpawnerSystem))]
[UpdateAfter(typeof(SpawnerSystem))]
[RequireMatchingQueriesForUpdate]
[BurstCompile]
partial struct ComputeShaderAnimatorSystem : ISystem {

    private bool _initialized;
    private NativeArray<float3> _positions;



    [BurstCompile]
    public void OnCreate(ref SystemState state) {
        state.RequireForUpdate<ECSComputeShaderTag>();
        state.RequireForUpdate<ECSComputeShaderAnimator>();
        state.RequireForUpdate<ComputeShaderData>();
    }



    public void OnUpdate(ref SystemState state) {
        EntityQuery query = SystemAPI.QueryBuilder().WithAll<LocalTransform>().Build();

        // Get the compute shader
        ECSComputeShaderAnimator animator = SystemAPI.GetSingleton<ECSComputeShaderAnimator>();
        ComputeShaderData computeShaderData = SystemAPI.ManagedAPI.GetSingleton<ComputeShaderData>(); // Since it is managed component, we need to get it from the ManagedAPI.
        ComputeShader computeShader = computeShaderData.ComputeShader;
        ComputeShaderManager computeShaderManager = new(computeShader, (Vector2)animator.Amplitude, (Vector2)animator.Frequency, (Vector2)animator.PhaseMultiplier);

        // Get the positions
        if (!_initialized) {
            _positions = new NativeArray<float3>(query.CalculateEntityCount(), Allocator.Persistent);

            GetPositionJob getPositionJob = new() {
                Positions = _positions
            };
            JobHandle jobHandle = getPositionJob.ScheduleParallel(state.Dependency); // No need to specify the query for IJobEntity, it will automatically create the query.
            jobHandle.Complete();

            _initialized = true;

        // Update the positions
        } else { 
            float3[] positions = _positions.ToArray();
            computeShaderManager.UpdatePositions(positions, (float)SystemAPI.Time.ElapsedTime);
            _positions.CopyFrom(positions);

            SetPositionJob setPositionJob = new() {
                Positions = _positions
            };
            setPositionJob.ScheduleParallel(); // No need to specify the query for IJobEntity, it will automatically create the query.
        }

    }



    [BurstCompile]
    public void OnDestroy(ref SystemState state) {
        _positions.Dispose();
    }



}



[BurstCompile]
public partial struct SetPositionJob : IJobEntity {

    [ReadOnly] public NativeArray<float3> Positions;



    [BurstCompile]
    // IJobEntity will automatically create the query of LocalTransform for us.
    public void Execute([EntityIndexInQuery] int index, ref LocalTransform localTransform) {
        localTransform.Position = Positions[index];
    }



}



[BurstCompile]
public partial struct GetPositionJob : IJobEntity {

    public NativeArray<float3> Positions;



    [BurstCompile]
    // IJobEntity will automatically create the query of LocalTransform for us.
    public void Execute([EntityIndexInQuery] int index, in LocalTransform localTransform) {
        Positions[index] = localTransform.Position;
    }



}

