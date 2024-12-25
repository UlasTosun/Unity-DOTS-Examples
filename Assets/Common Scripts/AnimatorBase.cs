using UnityEngine;



public class AnimatorBase : MonoBehaviour {

    [Header("Settings")]
    [Tooltip("The amplitude of the wave in the x and z directions.")]
    [Min(0f)]
    public Vector2 Amplitude = new (7f, 7f);
    [Tooltip("The frequency of the wave in the x and z directions.")]
    [Min(0f)]
    public Vector2 Frequency = new(0.1f, 0.1f);
    [Tooltip("The phase of the wave in the x and z directions.")]
    public Vector2 PhaseMultiplier = new(1f, 1f);

}
