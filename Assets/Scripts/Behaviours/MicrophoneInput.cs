using Meta.WitAi.Lib;
using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.Events;

// [RequireComponent(typeof(AudioSource))]
public class MicrophoneInput : MonoBehaviour
{
    [SerializeField]
    private int maxRecordingLength = 1;

    [SerializeField, Tooltip("Must be a power of 2")]
    private int numberOfSamples = 64;
    private float[] samples;

    private string deviceName;
    private AudioClip audioClip;

    private int minFrequency;
    private int maxFrequency;
    private int activeFrequency;

    public UnityEvent<int> onTickUpdate = new();
    public UnityEvent onRecordingStarted = new();
    public UnityEvent onRecordingStopped = new();

    private bool active = false;

    private void Awake()
    {
        string[] deviceNames = Microphone.devices;

        if (deviceNames.Length <= 0)
        {
            Debug.LogError("No microphone devices found!");
            return;
        }

        deviceName = deviceNames[0];
        Microphone.GetDeviceCaps(deviceName, out minFrequency, out maxFrequency);
        activeFrequency = Mathf.RoundToInt(minFrequency + (maxFrequency - minFrequency) / 2f);

        // audioSource = GetComponent<AudioSource>();
        samples = new float[numberOfSamples];
    }

    public void StartCapturing()
    {
        audioClip = Microphone.Start(
            deviceName,
            true,
            maxRecordingLength,
            activeFrequency
        );

        active = true;
        onRecordingStarted?.Invoke();
    }

    public void StopCapturing()
    {
        Microphone.End(deviceName);
        active = false;
        onRecordingStopped?.Invoke();
    }

    public void Update()
    {
        if (!active)
        {
            return;
        }
        
        GetMicrophoneVolume(deviceName);
        onTickUpdate?.Invoke(activeFrequency);
    }

    private float GetMicrophoneVolume(string microphoneName)
    {
        int position = Microphone.GetPosition(microphoneName);
        samples = new float[numberOfSamples];
        
        audioClip.GetData(samples, position - numberOfSamples);
        
        float volume = 0;
        for (int i = 0; i < numberOfSamples; i++)
        {
            volume += Mathf.Abs(samples[i]);
        }
        
        return volume / numberOfSamples;
    }
}