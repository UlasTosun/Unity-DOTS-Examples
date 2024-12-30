using UnityEngine;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;



[CreateAfter(typeof(SpawnerSystem))]
[RequireMatchingQueriesForUpdate]
[BurstCompile]
partial struct ComputeShaderAnimatorSystem : ISystem {

    private bool _initialized;
    private NativeArray<float3> _positions;



    [BurstCompile]
    public void OnCreate(ref SystemState state) {
        state.RequireForUpdate<ECSComputeShaderAnimator>();
        state.RequireForUpdate<ComputeShaderData>();
    }




    public void OnUpdate(ref SystemState state) {
        EntityQuery query = SystemAPI.QueryBuilder().WithAll<LocalTransform>().Build();

        // Get the compute shader
        ECSComputeShaderAnimator animator = SystemAPI.GetSingleton<ECSComputeShaderAnimator>();
        ComputeShaderData computeShaderData = SystemAPI.ManagedAPI.GetSingleton<ComputeShaderData>(); // Since it is managed component, we need to get it from the ManagedAPI
        ComputeShader computeShader = computeShaderData.ComputeShader;
        ComputeShaderManager computeShaderManager = new(computeShader, (Vector2)animator.Amplitude, (Vector2)animator.Frequency, (Vector2)animator.PhaseMultiplier);

        if (!_initialized) {            
            // Get the positions
            _positions = new NativeArray<float3>(query.CalculateEntityCount(), Allocator.Persistent);
            int i = 0;
            foreach (LocalTransform localTransform in query.ToComponentDataArray<LocalTransform>(Allocator.Temp)) {
                _positions[i] = localTransform.Position;
                i++;
            }

            _initialized = true;
        }

        // Update the positions
        float3[] positions = _positions.ToArray();
        computeShaderManager.UpdatePositions(positions, (float) SystemAPI.Time.ElapsedTime);
        _positions.CopyFrom(positions);

        SetPositionJob setPositionJob = new() {
            Positions = _positions
        };
        setPositionJob.ScheduleParallel(); // no need to specify the query for IJobEntity, it will automatically create the query

    }



    [BurstCompile]
    public void OnDestroy(ref SystemState state) {
        _positions.Dispose();
    }



}



[BurstCompile]
public partial struct SetPositionJob : IJobEntity {

    public NativeArray<float3> Positions;



    [BurstCompile]
    public void Execute([EntityIndexInQuery] int index, ref LocalTransform localTransform) {
        localTransform.Position = Positions[index];
    }



}
