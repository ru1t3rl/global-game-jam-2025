using BubblePuzzle.Puzzles;
using BubblePuzzle.Puzzles.Configuration;
using UnityEngine;
using UnityEngine.Events;

namespace BubblePuzzle.Behaviours
{
    public class BubbleInstantiater : MonoBehaviour
    {


        [SerializeField]
        private Transform spawnPoint;

        [SerializeField]
        private Transform bubbleContainer;
        
        public UnityEvent<Transform> onBubbleInstantiated;
        
        public int BubbleCount { get; private set; }= 0;

        [ContextMenu("Spawn Bubble")]
        public void InstantiateBubble()
        {
            BubbleCount++;
            GameObject bubble = Instantiate(PuzzleManager.Instance.activePuzzle.BubblePrefab, spawnPoint.position, Quaternion.identity, bubbleContainer);
            onBubbleInstantiated?.Invoke(bubble.transform);
        }
    }
}