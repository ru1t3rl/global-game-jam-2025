using System.Collections.Generic;
using BubblePuzzle.Puzzles.Configuration;
using UnityEngine;
using BubblePuzzle.Utilities;

namespace BubblePuzzle.Puzzles
{
    public class PuzzleManager : UnitySingleton<PuzzleManager>
    {
         [SerializeField]
        private List<Puzzle> puzzles = new List<Puzzle>();
        public Puzzle activePuzzle{ get; private set; }

        public List<Puzzle> Puzzles => puzzles;
        
        protected override void Awake()
        {
            base.Awake();
            activePuzzle = puzzles[0];
        }
        
        public void SetNextRandomPuzzle()
        {
            activePuzzle = puzzles[Random.Range(0, puzzles.Count)];
        }
        
        public Puzzle GetPuzzle(int index)
        {
            if (index >= 0 && index < puzzles.Count)
            {
                return puzzles[index];
            }
            return null;
        }

        public void AddPuzzle(Puzzle puzzle)
        {
            if (puzzle != null && !puzzles.Contains(puzzle))
            {
                puzzles.Add(puzzle);
            }
            else
            {
                Debug.LogWarning("Attempted to add a null or duplicate puzzle.");
            }
        }

        public void RemovePuzzle(Puzzle puzzle)
        {
            if (puzzles.Contains(puzzle))
            {
                puzzles.Remove(puzzle);
            }
            else
            {
                Debug.LogWarning("Attempted to remove a puzzle that is not in the list.");
            }
        }

        public int GetPuzzleCount()
        {
            return puzzles.Count;
        }

        public void ClearPuzzles()
        {
            puzzles.Clear();
        }
    }
}
