using Unity.Entities;
using UnityEngine;



class PhysicsSettingsAuthoring : MonoBehaviour {

    [Header("Settings")]
    [Tooltip("The target position to jump to.")]
    public Vector3 TargetPosition = new(0f, 100f, 0f);
    [Tooltip("The jump force to apply.")]
    public float JumpForce = 100f;

}



class PhysicsSettingsAuthoringBaker : Baker<PhysicsSettingsAuthoring> {



    public override void Bake(PhysicsSettingsAuthoring authoring) {
        Entity entity = GetEntity(TransformUsageFlags.None);

        PhysicsSettings settings = new() {
            TargetPosition = authoring.TargetPosition,
            JumpForce = authoring.JumpForce
        };

        AddComponent(entity, settings);
    }



}
