using UnityEngine;
using Unity.Mathematics;



public class DefaultComputeShaderAnimator : AnimatorBase {

    [Header("References")]
    [Tooltip("The prefab spawner to use.")]
    public PrefabSpawner PrefabSpawner;
    [Tooltip("The compute shader to animate with.")]
    public ComputeShader ComputeShader;

    private Transform[] _objectsToAnimate;
    private float3[] _positions;
    private ComputeShaderManager _computeShaderManager;



    void Start() {
        _objectsToAnimate = PrefabSpawner.SpawnedObjects;
        _positions = GetPositions(_objectsToAnimate);

        _computeShaderManager = new(ComputeShader, Amplitude, Frequency, PhaseMultiplier);
    }



    void Update() {
        _computeShaderManager.UpdatePositions(_positions, Time.time);
        SetPositions(_objectsToAnimate, _positions);
    }



    private float3[] GetPositions(Transform[] objectsToAnimate) {
        float3[] positions = new float3[objectsToAnimate.Length];
        for (int i = 0; i < objectsToAnimate.Length; i++) {
            positions[i] = objectsToAnimate[i].position;
        }
        return positions;
    }



    private void SetPositions(Transform[] objectsToAnimate, float3[] positions) {
        for (int i = 0; i < objectsToAnimate.Length; i++) {
            objectsToAnimate[i].position = positions[i];
        }
    }



}
