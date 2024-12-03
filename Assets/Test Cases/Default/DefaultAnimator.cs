using UnityEngine;



public class DefaultAnimator : AnimatorBase {



    void Update() {

        for (int i = 0; i < ObjectsToAnimate.Length; i++) {
            Transform objectToAnimate = ObjectsToAnimate[i];
            float xPosition = objectToAnimate.position.x;
            float zPosition = objectToAnimate.position.z;
            float xWave = Mathf.Sin(Frequency.x * xPosition + PhaseMultiplier.x * Time.time);
            float zWave = Mathf.Sin(Frequency.y * zPosition + PhaseMultiplier.y * Time.time);
            float yPosition = Amplitude.x * xWave + Amplitude.y * zWave;
            objectToAnimate.position = new Vector3(xPosition, yPosition, zPosition);
        }

    }


}
