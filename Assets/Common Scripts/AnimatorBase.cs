using UnityEngine;



public abstract class AnimatorBase : MonoBehaviour {

    [Header("References")]
    [Tooltip("The prefab spawner to use.")]
    [SerializeField] protected PrefabSpawner PrefabSpawner;

    [Header("Settings")]
    [Tooltip("The amplitude of the wave in the x and z directions.")]
    [Min(0f)]
    [SerializeField] protected Vector2 Amplitude = new (7f, 7f);
    [Tooltip("The frequency of the wave in the x and z directions.")]
    [Min(0f)]
    [SerializeField] protected Vector2 Frequency = new(0.1f, 0.1f);
    [Tooltip("The phase of the wave in the x and z directions.")]
    [SerializeField] protected Vector2 PhaseMultiplier = new(1f, 1f);

    [HideInInspector] public Transform[] ObjectsToAnimate => PrefabSpawner.SpawnedObjects;



}
