using UnityEngine;



public class PhysicsDefault : MonoBehaviour {

    [Header("References")]
    [Tooltip("The prefab spawner to use.")]
    public PrefabSpawner PrefabSpawner;

    [Header("Settings")]
    [Tooltip("The target position to jump to.")]
    [SerializeField] private Vector3 _targetPosition = new (0f, 100f, 0f);
    [Tooltip("The jump force to apply.")]
    [SerializeField] private float _jumpForce = 100f;



    void Start() {
        Jump();
    }



    private void Jump() {
        Transform[] objectsToAnimate = PrefabSpawner.SpawnedObjects;

        for (int i = 0; i < objectsToAnimate.Length; i++) {
            Transform objectToAnimate = objectsToAnimate[i];
            Rigidbody rigidbody = objectToAnimate.GetComponent<Rigidbody>();
            Vector3 direction = (_targetPosition - objectToAnimate.position).normalized;
            rigidbody.AddForce(direction * _jumpForce, ForceMode.Impulse);
        }
    }





}
