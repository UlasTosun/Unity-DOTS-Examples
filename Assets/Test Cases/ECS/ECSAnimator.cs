using Unity.Entities;
using Unity.Mathematics;



public struct ECSAnimator : IComponentData {

    public float2 Amplitude;
    public float2 Frequency;
    public float2 PhaseMultiplier;
    
}
