using System.Collections.Generic;
using System.Linq;
using BubblePuzzle.Puzzles.Configuration;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace BubblePuzzle.Puzzles.Validation
{
    [RequireComponent(typeof(MeshComparer))]
    public class PuzzleValidator : MonoBehaviour
    {
        
        [SerializeField]
        private Transform bubbleContainer;

        public UnityEvent<ValidationResult> OnValidationFinished;
        
        private MeshComparer _meshComparer;
        
        public UnityEvent onValidationSucceeded;

        private void Awake()
        {
            _meshComparer = GetComponent<MeshComparer>();
        }
        
        public void ValidationForButton(){
            Validate();            
        }

        public ValidationResult Validate()
        {
            if (PuzzleManager.Instance.activePuzzle == null)
            {
                Debug.LogError("No puzzle is set!");
                return new ValidationResult
                {
                    IsValid = false
                };
            }

            Transform[] bubbles = GetAllBubblesFromContainer();

            if (bubbles.Length == 0)
            {
                return new ValidationResult
                {
                    IsValid = false
                };
            }

            float matchPercentage = _meshComparer.ValidateShape();

            bool isValid = matchPercentage > 0.8f;
            if (isValid)
            {
                onValidationSucceeded?.Invoke();
            }
            
            return new ValidationResult
            {
                IsValid = isValid,
                MatchPercentage = matchPercentage,
                TotalBubbles = bubbles.Length
            };
        }

        private Transform[] GetAllBubblesFromContainer()
        {
            Transform[] bubbles = new Transform[bubbleContainer.childCount];
            for (int i = 0; i < bubbles.Length; i++)
            {
                bubbles[i] = bubbleContainer.GetChild(i);
            }
            return bubbles;
        }
    }
}