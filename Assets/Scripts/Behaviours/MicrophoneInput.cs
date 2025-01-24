using Meta.WitAi.Lib;
using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class MicrophoneInput : MonoBehaviour
{
    [SerializeField]
    private int maxRecordingLength = 1;

    [SerializeField, Tooltip("Must be a power of 2")]
    private int numberOfSamples = 64;
    private float[] _samples;

    [SerializeField]
    private float volumeMultiplier = 10.0f;
    
    [SerializeField, Tooltip("The threshold won't be multiplied!")]
    private float volumeThreshold = 1;

    private string _deviceName;
    private AudioClip _audioClip;

    private int _minFrequency;
    private int _maxFrequency;
    private int _activeFrequency;

    public UnityEvent<float> onTickUpdate = new();
    public UnityEvent onRecordingStarted = new();
    public UnityEvent onRecordingStopped = new();

    private bool _active = false;

    private void Awake()
    {
        string[] deviceNames = Microphone.devices;

        if (deviceNames.Length <= 0)
        {
            Debug.LogError("No microphone devices found!");
            return;
        }

        _deviceName = deviceNames[0];
        Debug.Log($"Using audio device: {_deviceName}");
        Microphone.GetDeviceCaps(_deviceName, out _minFrequency, out _maxFrequency);
        _activeFrequency = Mathf.RoundToInt(_minFrequency + (_maxFrequency - _minFrequency) / 2f);

        _samples = new float[numberOfSamples];
    }

    [ContextMenu("Start Microphone")]
    public void StartCapturing()
    {
        _audioClip = Microphone.Start(
            _deviceName,
            true,
            maxRecordingLength,
            _activeFrequency
        );

        _active = true;
        onRecordingStarted?.Invoke();
    }

    [ContextMenu("Stop Microphone")]
    public void StopCapturing()
    {
        Microphone.End(_deviceName);
        _active = false;
        onRecordingStopped?.Invoke();
    }

    public void Update()
    {
        if (!_active)
        {
            return;
        }

        float volume = GetMicrophoneVolume(_deviceName) * volumeMultiplier;

        if (volume >= volumeThreshold)
        {
            onTickUpdate?.Invoke(volume);
        }
    }

    private float GetMicrophoneVolume(string microphoneName)
    {
        int position = Microphone.GetPosition(microphoneName);
        _samples = new float[numberOfSamples];

        _audioClip.GetData(_samples, Mathf.Max(0, position - numberOfSamples));

        float volume = 0;
        for (int i = 0; i < numberOfSamples; i++)
        {
            volume += Mathf.Abs(_samples[i]);
        }

        return volume / numberOfSamples;
    }
}