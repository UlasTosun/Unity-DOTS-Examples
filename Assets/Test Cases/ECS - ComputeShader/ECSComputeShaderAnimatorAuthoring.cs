using Unity.Entities;



public class ECSComputeShaderAnimatorAuthoring : AnimatorBase {

}



public class ECSComputeShaderAnimatorBaker : Baker<ECSComputeShaderAnimatorAuthoring> {



    public override void Bake(ECSComputeShaderAnimatorAuthoring authoring) {
        Entity entity = GetEntity(TransformUsageFlags.None);

        ECSComputeShaderAnimator animator = new () {
            Amplitude = authoring.Amplitude,
            Frequency = authoring.Frequency,
            PhaseMultiplier = authoring.PhaseMultiplier
        };

        AddComponent(entity, animator);
    }



}
