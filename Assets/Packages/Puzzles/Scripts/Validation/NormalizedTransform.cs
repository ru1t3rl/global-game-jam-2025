using UnityEngine;

namespace BubblePuzzle.Puzzles.Validation
{
    public record NormalizedTransform(
        Vector3 Position,
        Quaternion Rotation,
        Vector3 Scale
    )
    {
        public Quaternion Rotation { get; } = Rotation;
        public Vector3 Scale { get; } = Scale;
        public Vector3 Position { get; } = Position;
    }
}