using BubblePuzzle.Puzzles.Configuration;
using UnityEngine;

namespace BubblePuzzle.Behaviours
{
    public class BubbleInstantiater : MonoBehaviour
    {
        [SerializeField]
        private Puzzle puzzle;

        [SerializeField]
        private Transform spawnPoint;

        [SerializeField]
        private Transform bubbleContainer;

        public int BubbleCount { get; private set; }= 0;

        [ContextMenu("Spawn Bubble")]
        public void InstantiateBubble()
        {
            BubbleCount++;
            Instantiate(puzzle.BubblePrefab, spawnPoint.position, Quaternion.identity, bubbleContainer);
        }
    }
}