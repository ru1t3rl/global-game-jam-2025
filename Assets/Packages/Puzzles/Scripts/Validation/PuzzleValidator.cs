using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace BubblePuzzle.Puzzles.Validation
{
    public class PuzzleValidator : MonoBehaviour
    {
        [SerializeField]
        private float positionTolerance = 0.1f;
        [SerializeField]
        private float rotationTolerance = 0.1f;
        [SerializeField]
        private float sizeTolerance = 0.1f;

        public UnityEvent<ValidationResult> OnValidationFinished;

        private Puzzle _puzzle;

        public ValidationResult Validate(List<Transform> bubbles)
        {
            if (_puzzle == null)
            {
                Debug.LogError("No puzzle is set!");
                return new ValidationResult()
                {
                    IsValid = false
                };
            }

            Vector3 center = CalculateCenter(bubbles);
            List<NormalizedTransform> normalizedBubbles = NormalizeBubbles(bubbles, center);

            float matchPercentage = CalculateOverallMatchPercentage(normalizedBubbles, _puzzle.Bubbles.ToList());
            return new ValidationResult
            {
                IsValid = matchPercentage > 0.8f,
                MatchPercentage = matchPercentage,
                TotalBubbles = bubbles.Count
            };
        }

        private List<NormalizedTransform> NormalizeBubbles(List<Transform> bubbles, Vector3 center)
        {
            float maxScale = bubbles.Max(b => b.localScale.magnitude);
            return bubbles.Select(b => new NormalizedTransform(
                    (b.transform.position - center),
                    b.transform.rotation,
                    b.transform.localScale / maxScale
                )
            ).ToList();
        }

        private Vector3 CalculateCenter(List<Transform> bubbles)
        {
            return bubbles.Aggregate(
                Vector3.zero,
                (acc, bubble) => acc + bubble.position
            ) / bubbles.Count;
        }

        private float CalculateOverallMatchPercentage(
            List<NormalizedTransform> normalizedBubbles,
            List<Transform> targetReferences)
        {
            int matchedBubbles = 0;
            foreach (var bubble in normalizedBubbles)
            {
                var matchingReference = targetReferences.FirstOrDefault(reference =>
                    Vector3.Distance(bubble.Position, reference.position) <= positionTolerance &&
                    Quaternion.Angle(bubble.Rotation, reference.rotation) <= rotationTolerance &&
                    Mathf.Abs(bubble.Scale.magnitude - reference.localScale.magnitude) <= sizeTolerance
                );

                if (matchingReference != null)
                    matchedBubbles++;
            }

            return (float)matchedBubbles / targetReferences.Count;
        }

        public void SetPuzzle(Puzzle puzzle)
        {
            _puzzle = puzzle;
        }
    }
}