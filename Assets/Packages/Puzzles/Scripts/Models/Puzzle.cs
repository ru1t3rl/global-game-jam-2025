using UnityEngine;

namespace BubblePuzzle.Puzzles.Configuration
{
    [CreateAssetMenu(fileName = "Puzzle", menuName = "Puzzles/Puzzle")]
    public class Puzzle : ScriptableObject
    {
        [SerializeField]
        private int maxBubbles = 99;
        
        [SerializeField]
        private GameObject bubblePrefab; 

        [SerializeField]
        private BubbleTransform[] bubbleShape;
        
        public int MaxBubbles => maxBubbles;
        public GameObject BubblePrefab => bubblePrefab;
        public BubbleTransform[] BubbleShape => bubbleShape;

        public void SetBubbleShape(BubbleTransform[] shape)
        {
            bubbleShape = shape;
        }
    }
}