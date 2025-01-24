using System.Collections.Generic;
using UnityEngine;

namespace BubblePuzzle.Puzzles
{
    public class Puzzle : MonoBehaviour
    {
        [SerializeField]
        private bool fromChildren = true;
        
        private Transform[] bubbles;
        public Transform[] Bubbles => bubbles;

        private void Awake()
        {
            bubbles = new Transform[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
            {
                bubbles[i] = transform.GetChild(i);
            }
        }
    }
}