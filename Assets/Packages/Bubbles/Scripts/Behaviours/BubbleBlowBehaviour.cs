using BubblePuzzle.Enums;
using UnityEngine;

namespace BubblePuzzle.Behaviours
{
    public class BubbleBlowBehaviour : MonoBehaviour
    {
        [SerializeField, Tooltip("When left empty, the object the script is attached to will be used as the bubble")]
        private GameObject bubbleObject;

        [Header("Configuration")]
        [SerializeField]
        private GrowMethod method = GrowMethod.Logarithmic;
        [SerializeField]
        private float growMultiplier = 1.0f;

        private float _totalVolume = 0;
        private float _size;

        private Vector3 _originalScale;

        private void Awake()
        {
            if (bubbleObject == null)
            {
                bubbleObject = gameObject;
            }

            _originalScale = bubbleObject.transform.localScale;

            MicrophoneInput.Instance.onUpdate?.AddListener(Grow);
        }

        public void Grow(float amount)
        {
            _totalVolume += amount;
            _size = method switch
            {
                GrowMethod.Linear => _totalVolume,
                GrowMethod.Logarithmic => Mathf.Log(_totalVolume, 2),
                _ => throw new System.NotImplementedException($"The selected GrowMethod is not implemented yet. ({method})")
            };

            _size *= growMultiplier;
            bubbleObject.transform.localScale = _originalScale * _size;
        }
    }
}