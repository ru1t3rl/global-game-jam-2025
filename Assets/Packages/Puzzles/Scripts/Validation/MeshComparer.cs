using BubblePuzzle.Puzzles.Configuration;
using UnityEngine;

namespace BubblePuzzle.Puzzles.Validation
{
    public class MeshComparer : MonoBehaviour
    {
        public Puzzle puzzle;
        private Transform puzzelTargetShape;

        [SerializeField]
        private Transform bubbleContainer; // Parent object containing multiple meshes

        public float ValidateShape()
        {
            SpawnTargetShape();
            (float matchPercentage, float totalDistance) =
                CalculateMatchPercentageAndDistance(puzzelTargetShape, bubbleContainer);

            DestroyTargetShape();

            return matchPercentage;
        }

        private void SpawnTargetShape()
        {
            puzzelTargetShape = new GameObject().transform;
            for (int i = 0; i < puzzle.BubbleShape.Length; i++)
            {
                var shape = Instantiate(puzzle.BubblePrefab, Vector3.zero, Quaternion.identity, puzzelTargetShape).transform;
                shape.SetParent(puzzelTargetShape);
                shape.localPosition = puzzle.BubbleShape[i].Position;
                shape.localRotation = puzzle.BubbleShape[i].Rotation;
                shape.localScale = puzzle.BubbleShape[i].Scale;
            }
        }

        private void DestroyTargetShape()
        {
            Destroy(puzzelTargetShape.gameObject);
        }

        private (float, float) CalculateMatchPercentageAndDistance(Transform shapeA, Transform shapeB)
        {
            // Check if the number of child meshes is the same
            if (shapeA.childCount != shapeB.childCount)
                return (0f, float.MaxValue); // Early exit if counts differ

            int totalComponents = shapeA.childCount;
            int matchCount = 0;
            float totalDistance = 0f;

            // Compare each mesh
            for (int i = 0; i < totalComponents; i++)
            {
                Mesh meshA = shapeA.GetChild(i).GetComponent<MeshFilter>().mesh;
                Mesh meshB = shapeB.GetChild(i).GetComponent<MeshFilter>().mesh;

                // Get the scales of the parent transforms
                Vector3 scaleA = shapeA.GetChild(i).localScale;
                Vector3 scaleB = shapeB.GetChild(i).localScale;

                if (CompareMeshes(meshA, meshB, scaleA, scaleB, out float distance))
                {
                    matchCount++;
                }
                else
                {
                    totalDistance += distance; // Accumulate distance for non-matching meshes
                }
            }

            // Calculate match percentage
            float matchPercentage = (float)matchCount / totalComponents; // Convert to percentage

            return (matchPercentage, totalDistance);
        }

        private bool CompareMeshes(Mesh meshA, Mesh meshB, Vector3 scaleA, Vector3 scaleB, out float distance)
        {
            distance = 0f; // Initialize distance

            if (meshA.vertexCount != meshB.vertexCount)
                return false;

            Vector3[] verticesA = meshA.vertices;
            Vector3[] verticesB = meshB.vertices;

            // Compare vertices and calculate distance, accounting for scale
            for (int i = 0; i < verticesA.Length; i++)
            {
                // Adjust vertex positions based on scale
                Vector3 scaledVertexA = Vector3.Scale(verticesA[i], scaleA);
                Vector3 scaledVertexB = Vector3.Scale(verticesB[i], scaleB);

                if (scaledVertexA != scaledVertexB)
                {
                    distance += Vector3.Distance(scaledVertexA, scaledVertexB); // Accumulate distance
                }
            }

            // If distance is zero, the meshes match
            return distance == 0f;
        }
    }
}