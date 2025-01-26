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
        private BubbleBlowBehaviour _attachedBlowBehaviour;
        private Transform _previousBubbleParent;

        [SerializeField]
        private InputActionReference[] bubbleReleaseActionReference;

        private void Awake()
        {
            _bubbleInstantiater ??= GetComponent<BubbleInstantiater>();
        }

        private void OnEnable()
        {
            for (int i = 0; i < bubbleReleaseActionReference.Length; i++)
            {
                bubbleReleaseActionReference[i].action.performed += ReleaseBubble;
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < bubbleReleaseActionReference.Length; i++)
            {
                bubbleReleaseActionReference[i].action.performed -= ReleaseBubble;
            }
        }

        public void StartBlowing()
        {
            MicrophoneInput.Instance.onUpdate?.AddListener(OnMicrophoneInput);
        }

        public void StopBlowing()
        {
            MicrophoneInput.Instance.onUpdate?.RemoveListener(OnMicrophoneInput);
        }

        private void OnMicrophoneInput(float volume)
        {
            if (volume < newBubbleThreshold)
            {
                Debug.LogWarning($"To Quiet: {volume}");
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

            AttachBubble(other.transform);
        }

        public void AttachBubble(Transform bubbleTransform)
        {
            MicrophoneInput.Instance.onUpdate?.RemoveListener(OnMicrophoneInput);
            _attachedBubble = bubbleTransform.gameObject;
            _previousBubbleParent = bubbleTransform.parent;
            _attachedBubble.transform.SetParent(transform);


            _attachedBlowBehaviour = bubbleTransform.GetComponent<BubbleBlowBehaviour>();
            if (_attachedBlowBehaviour == null)
            {
                Debug.LogWarning(
                    $"The object attached to the wand doesn't have a BlowBehaviour so won't be able to grow. ({nameof(bubbleTransform)})");

                return;
            }

            _attachedBlowBehaviour.Attach();
        }

        [ContextMenu("Release Bubble")]
        public void ReleaseBubble(InputAction.CallbackContext context = default)
        {
            _attachedBubble?.transform.SetParent(_previousBubbleParent);
            _attachedBubble = null;
            _previousBubbleParent = null;

            MicrophoneInput.Instance.onUpdate?.AddListener(OnMicrophoneInput);

            if (_attachedBlowBehaviour)
            {
                _attachedBlowBehaviour.Detach();
                _attachedBlowBehaviour = null;
            }
        }
    }
}