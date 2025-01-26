using UnityEngine;

namespace BubblePuzzle.Utilities
{

    [CreateAssetMenu(fileName = "MicConfiguration", menuName = "Audio/Microphone Configuration")]
    public class MicrophoneConfiguration : ScriptableObject
    {
        [field: SerializeField]
        public int DeviceIndex { get; set; }

        [field: SerializeField]
        public float InputMultiplier { get; set; }
    }
}