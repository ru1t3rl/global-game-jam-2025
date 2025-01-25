using BubblePuzzle.Behaviours;
using UnityEngine;

namespace BubblePuzzle.Bubbles.MicrophoneUtilities
{
    [RequireComponent(typeof(SphereCollider))]
    public class MicrophoneTrigger : MonoBehaviour
    {
        [SerializeField]
        private GameObject wandGameObject;
        
        private SphereCollider _collider;

        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(wandGameObject.tag))
            {
                return;
            } 
            
            MicrophoneInput.Instance.StartCapturing();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(wandGameObject.tag))
            {
                return;
            }
            
            MicrophoneInput.Instance.StopCapturing();
        }
    }
}