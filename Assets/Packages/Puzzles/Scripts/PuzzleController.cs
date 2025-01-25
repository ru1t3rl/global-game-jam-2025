using BubblePuzzle.Puzzles.Configuration;
using UnityEngine;

namespace BubblePuzzle.Puzzles
{
    public class PuzzleController : MonoBehaviour
    {
        [SerializeField]
        private Puzzle puzzle;
        public Puzzle Puzzle => puzzle;
        
        [SerializeField]
        private bool fromChildren = true;
        
        [SerializeField]
        private int maxBubbles = 99;

        public void SaveToConfiguration()
        {
            BubbleTransform[] bubbles = new BubbleTransform[transform.childCount];
            Transform t;

            for (int i = 0; i < bubbles.Length; i++)
            {
                t = transform.GetChild(i);
                bubbles[i] = new BubbleTransform
                {
                    Position = t.localPosition,
                    Rotation = t.localRotation,
                    Scale = t.localScale
                };
            }

            puzzle.SetBubbleShape(bubbles);
        }
    }
}