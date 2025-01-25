using System;
using BubblePuzzle.Behaviours;
using BubblePuzzle.Puzzles.Configuration;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace BubblePuzzle
{
    public class WandBehaviour : MonoBehaviour
    {
        [SerializeField]
        private string bubbleTag = "Bubble";

        [SerializeField]
        private float newBubbleThreshold = 1.0f;

        [SerializeField]
        private BubbleInstantiater _bubbleInstantiater;

        [CanBeNull]
        private GameObject _attachedBubble = null;
        public GameObject AttachedBubble => _attachedBubble;
        private Transform previousBubbleParent = null;

        [SerializeField]
        private InputActionReference[] bubbleReleaseActionReference;

        private void Awake()
        {
            _bubbleInstantiater ??= GetComponent<BubbleInstantiater>();
        }

        private void OnEnable()
        {
            MicrophoneInput.Instance.onUpdate?.AddListener(OnMicrophoneInput);
            
            for(int i = 0; i < bubbleReleaseActionReference.Length; i++) { 
                bubbleReleaseActionReference[i].action.performed += ReleaseBubble;
            }
        }

        private void OnDisable()
        {
            MicrophoneInput.Instance.onUpdate?.RemoveListener(OnMicrophoneInput);
            
            for(int i = 0; i < bubbleReleaseActionReference.Length; i++) {
                bubbleReleaseActionReference[i].action.performed -= ReleaseBubble;
            }
        }

        private void OnMicrophoneInput(float volume)
        {
            Debug.Log("Volume: "+volume);
            if (volume < newBubbleThreshold)
            {
                return;
            }

            _bubbleInstantiater?.InstantiateBubble();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (_attachedBubble != null || !other.gameObject.CompareTag(bubbleTag))
            {
                return;
            }
            
            Debug.Log("Bubble Collision");

            MicrophoneInput.Instance.onUpdate?.RemoveListener(OnMicrophoneInput);
            _attachedBubble = other.gameObject;
            previousBubbleParent = other.gameObject.transform.parent;
            _attachedBubble.transform.SetParent(transform);
        }

        [ContextMenu("Release Bubble")]
        public void ReleaseBubble(InputAction.CallbackContext context = default)
        {
            _attachedBubble?.transform.SetParent(previousBubbleParent);
            _attachedBubble = null;
            previousBubbleParent = null;
            
            MicrophoneInput.Instance.onUpdate?.AddListener(OnMicrophoneInput);
        }
    }
}