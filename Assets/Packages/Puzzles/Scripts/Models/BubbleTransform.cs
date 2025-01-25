using System;
using UnityEngine;

namespace BubblePuzzle.Puzzles.Configuration
{
    [Serializable]
    public struct BubbleTransform
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;
    }
}