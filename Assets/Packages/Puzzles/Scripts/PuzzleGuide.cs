using System.Collections.Generic;
using System.Linq;
using BubblePuzzle.Puzzles.Configuration;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace BubblePuzzle.Puzzles.Guide
{
    public class PuzzleGuide : MonoBehaviour
    {
        [SerializeField]
        private GameObject previewBubblePrefab;

        [SerializeField]
        private LayerMask previewLayerMask;

        [SerializeField]
        private Transform previewBubblesContainer;

        [SerializeField]
        private Camera previewCamera;

        private void OnEnable()
        {
            SpawnBubblesForPreview();
            previewCamera.transform.LookAt(previewBubblesContainer);
        }

        private void SpawnBubblesForPreview()
        {
            previewBubblesContainer.SetParent(null);
            previewBubblesContainer.position = Vector3.zero;
            previewBubblesContainer.rotation = Quaternion.identity;
            previewBubblesContainer.localScale = Vector3.one;

            List<Transform> bubbles = new ();
            for (int i = 0; i < PuzzleManager.Instance.activePuzzle.BubbleShape.Length; i++)
            {
                var gobj = Instantiate(previewBubblePrefab, Vector3.zero, Quaternion.identity, previewBubblesContainer );
                gobj.transform.localPosition = PuzzleManager.Instance.activePuzzle.BubbleShape[i].Position;
                gobj.transform.localRotation = PuzzleManager.Instance.activePuzzle.BubbleShape[i].Rotation;
                gobj.transform.localScale = PuzzleManager.Instance.activePuzzle.BubbleShape[i].Scale;
                
                gobj.layer = (int) Mathf.Log(previewLayerMask.value, 2);
                bubbles.Add(gobj.transform);
            }
            
            previewBubblesContainer.SetParent(transform);
            var center = CalculateCenter(bubbles.ToArray());
            previewCamera.transform.position = center;
            previewCamera.transform.position += new Vector3(0, 0, 5f);
        }
        
        private Vector3 CalculateCenter(Transform[] bubbles)
        {
            return bubbles.Aggregate(
                Vector3.zero,
                (acc, bubble) => acc + bubble.position
            ) / bubbles.Length;
        }
    }
}
