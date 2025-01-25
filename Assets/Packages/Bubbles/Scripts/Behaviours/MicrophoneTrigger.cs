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
            if (!other.gameObject.CompareTag(wandGameObject.tag))
            {
                return;
            } 
            
            Debug.Log("Start Captuing");
            MicrophoneInput.Instance.StartCapturing();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag(wandGameObject.tag))
            {
                return;
            }
            
            Debug.Log("Stop Capturing");
            MicrophoneInput.Instance.StopCapturing();
        }
    }
}