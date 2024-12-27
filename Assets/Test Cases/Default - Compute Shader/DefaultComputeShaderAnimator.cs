using UnityEngine;



public class DefaultComputeShaderAnimator : AnimatorBase {

    [Header("References")]
    [Tooltip("The prefab spawner to use.")]
    public PrefabSpawner PrefabSpawner;
    [Tooltip("The compute shader to animate with.")]
    public ComputeShader ComputeShader;

    private Transform[] _objectsToAnimate;
    private Vector3[] _positions;



    void Start() {
        _objectsToAnimate = PrefabSpawner.SpawnedObjects;
        _positions = GetPositions(_objectsToAnimate);
    }



    void Update() {
        UpdatePositions(_positions);
        SetPositions(_objectsToAnimate, _positions);
    }



    private void UpdatePositions(Vector3[] positions) {
        ComputeShader.GetKernelThreadGroupSizes(0, out uint threadsX, out uint threadsY, out uint threadsZ); // Get the thread group sizes.
        int stride = 12; // 3 floats (Vector3) = 12 bytes.

        ComputeBuffer computeBuffer = new(positions.Length, stride, ComputeBufferType.Default); // Create a compute buffer with the size of the positions array.
        computeBuffer.SetData(positions); // Set the data of the compute buffer to the positions array.

        ComputeShader.SetBuffer(0, "PositionBuffer", computeBuffer); // Set the compute buffer to the compute shader.

        // Set the compute shader properties.
        ComputeShader.SetVector("Amplitude", Amplitude);
        ComputeShader.SetVector("Frequency", Frequency);
        ComputeShader.SetVector("PhaseMultiplier", PhaseMultiplier);
        ComputeShader.SetFloat("ElapsedTime", Time.time);

        int threadGroupsX = Mathf.CeilToInt(positions.Length / (float)threadsX); // Calculate the number of thread groups in the X direction.
        ComputeShader.Dispatch(0, threadGroupsX, (int)threadsY, (int)threadsZ); // Dispatch the compute shader.

        computeBuffer.GetData(positions); // Get the data from the compute buffer back to the positions array.
        computeBuffer.Release(); // Release the compute buffer after the dispatch to free up memory.
    }



    private Vector3[] GetPositions(Transform[] objectsToAnimate) {
        Vector3[] positions = new Vector3[objectsToAnimate.Length];
        for (int i = 0; i < objectsToAnimate.Length; i++) {
            positions[i] = objectsToAnimate[i].position;
        }
        return positions;
    }



    private void SetPositions(Transform[] objectsToAnimate, Vector3[] positions) {
        for (int i = 0; i < objectsToAnimate.Length; i++) {
            objectsToAnimate[i].position = positions[i];
        }
    }



}
