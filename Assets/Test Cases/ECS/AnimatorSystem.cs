using Unity.Burst;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;



[CreateAfter(typeof(SpawnerSystem))]
[RequireMatchingQueriesForUpdate]
[BurstCompile]
partial struct AnimatorSystem : ISystem {



    [BurstCompile]
    public void OnCreate(ref SystemState state) {
        
    }



    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        EntityQuery query = SystemAPI.QueryBuilder().WithAll<ECSAnimator, LocalTransform>().Build();

        AnimatorJob animatorJob = new() {
            ElapsedTime = SystemAPI.Time.ElapsedTime
        };

        animatorJob.ScheduleParallel(query);
    }



    [BurstCompile]
    public void OnDestroy(ref SystemState state) {
        
    }



}



[BurstCompile]
public partial struct AnimatorJob : IJobEntity {

    [ReadOnly] public double ElapsedTime;



    [BurstCompile]
    public void Execute(ref ECSAnimator animator, ref LocalTransform localTransform) {
        float x = localTransform.Position.x;
        float z = localTransform.Position.z;

        float xWave = math.sin(animator.Frequency.x * x + animator.PhaseMultiplier.x * (float) ElapsedTime);
        float zWave = math.sin(animator.Frequency.y * z + animator.PhaseMultiplier.y * (float) ElapsedTime);

        float y = animator.Amplitude.x * xWave + animator.Amplitude.y * zWave;
        localTransform.Position = new float3(x, y, z);
    }



}
