using System.Text;
using UnityEngine;
using TMPro;



public class FPSMeter : MonoBehaviour {

    [Header("References")]
    [Tooltip("Text component to print the FPS.")]
    [SerializeField] private TextMeshProUGUI _text;

    [Header("Settings")]
    [Tooltip("The number of frames to average over.")]
    [SerializeField] private int _filterSize = 30;

    private float[] _deltaTimes;
    private readonly StringBuilder _stringBuilder = new();



    private void Awake() {
        _deltaTimes = new float[_filterSize];
    }



    void Update() {
        AddToFilter(Time.deltaTime);
        _stringBuilder.Clear();
        _text.text = _stringBuilder.Append("FPS: ").Append((1f / GetAverageDeltaTime()).ToString("0")).ToString();
    }



    private void AddToFilter(float value) {
        for (int i = 0; i < _filterSize - 1; i++) {
            _deltaTimes[i] = _deltaTimes[i + 1];
        }
        _deltaTimes[_filterSize - 1] = value;
    }



    private float GetAverageDeltaTime() {
        float sum = 0f;
        for (int i = 0; i < _filterSize; i++)
            sum += _deltaTimes[i];
        return sum / _filterSize;
    }

    

}
