using UnityEngine;
using Unity.Mathematics;



public class ComputeShaderManager {

    private ComputeShader _computeShader;
    


    public ComputeShaderManager(ComputeShader computeShader, Vector3 amplitude, Vector3 frequency, Vector3 phaseMultiplier) {
        _computeShader = computeShader;
        
        // Set the compute shader properties.
        _computeShader.SetVector("Amplitude", amplitude);
        _computeShader.SetVector("Frequency", frequency);
        _computeShader.SetVector("PhaseMultiplier", phaseMultiplier);
    }



    public void UpdatePositions(float3[] positions, float elapsedTime) {
        _computeShader.GetKernelThreadGroupSizes(0, out uint threadsX, out uint threadsY, out uint threadsZ); // Get the thread group sizes.
        int stride = 12; // 3 floats (Vector3) = 12 bytes.

        ComputeBuffer computeBuffer = new(positions.Length, stride, ComputeBufferType.Default); // Create a compute buffer with the size of the positions array.
        computeBuffer.SetData(positions); // Set the data of the compute buffer to the positions array.

        _computeShader.SetBuffer(0, "PositionBuffer", computeBuffer); // Set the compute buffer to the compute shader.
        _computeShader.SetFloat("ElapsedTime", elapsedTime);

        int threadGroupsX = Mathf.CeilToInt(positions.Length / (float)threadsX); // Calculate the number of thread groups in the X direction.
        _computeShader.Dispatch(0, threadGroupsX, (int)threadsY, (int)threadsZ); // Dispatch the compute shader.

        computeBuffer.GetData(positions); // Get the data from the compute buffer back to the positions array.
        computeBuffer.Release(); // Release the compute buffer after the dispatch to free up memory.
    }

    

}
