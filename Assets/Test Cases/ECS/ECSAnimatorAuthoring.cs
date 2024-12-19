using Unity.Entities;



public class ECSAnimatorAuthoring : AnimatorBase {

}



public class ECSAnimatorBaker : Baker<ECSAnimatorAuthoring> {



    public override void Bake(ECSAnimatorAuthoring authoring) {
        Entity entity = GetEntity(TransformUsageFlags.None);

        ECSAnimator animator = new ECSAnimator() {
            Amplitude = authoring.Amplitude,
            Frequency = authoring.Frequency,
            PhaseMultiplier = authoring.PhaseMultiplier
        };

        AddComponent(entity, animator);
    }



}
