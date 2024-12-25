using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine.Jobs;
using Unity.Burst;



public class JobsAnimator : AnimatorBase {

    [Header("References")]
    [Tooltip("The prefab spawner to use.")]
    public PrefabSpawner PrefabSpawner;

    private JobHandle _jobHandle;
    private JobsAnimatorStruct _jobStruct;
    private NativeArray<float2> _amplitude;
    private NativeArray<float2> _frequency;
    private NativeArray<float2> _phaseMultiplier;
    private TransformAccessArray _transformAccessArray;



    void Start() {
        _amplitude = new NativeArray<float2>(1, Allocator.Persistent);
        _amplitude[0] = Amplitude;

        _frequency = new NativeArray<float2>(1, Allocator.Persistent);
        _frequency[0] = Frequency;

        _phaseMultiplier = new NativeArray<float2>(1, Allocator.Persistent);
        _phaseMultiplier[0] = PhaseMultiplier;

        _transformAccessArray = new TransformAccessArray(PrefabSpawner.SpawnedObjects);

    }



    void Update() {
        _jobStruct = new JobsAnimatorStruct {
            Amplitude = _amplitude,
            Frequency = _frequency,
            PhaseMultiplier = _phaseMultiplier,
            Time = Time.time
        };

        _jobHandle = _jobStruct.Schedule(_transformAccessArray); 
    }



    void LateUpdate() {
        _jobHandle.Complete();
    }



    void OnDestroy() {
        _amplitude.Dispose();
        _frequency.Dispose();
        _phaseMultiplier.Dispose();
        _transformAccessArray.Dispose(); 
    }



}



[BurstCompile]
public struct JobsAnimatorStruct : IJobParallelForTransform {

    [ReadOnly] public NativeArray<float2> Amplitude;
    [ReadOnly] public NativeArray<float2> Frequency;
    [ReadOnly] public NativeArray<float2> PhaseMultiplier;
    [ReadOnly] public float Time;



    public void Execute(int i, TransformAccess transform) {
        float xPosition = transform.position.x;
        float zPosition = transform.position.z;
        float xWave = math.sin(Frequency[0].x * xPosition + PhaseMultiplier[0].x * Time);
        float zWave = math.sin(Frequency[0].y * zPosition + PhaseMultiplier[0].y * Time);
        float yPosition = Amplitude[0].x * xWave + Amplitude[0].y * zWave;
        transform.position = new float3(xPosition, yPosition, zPosition);
    }



}
