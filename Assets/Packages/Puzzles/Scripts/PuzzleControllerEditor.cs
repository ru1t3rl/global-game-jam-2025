using UnityEditor;
using UnityEngine;

namespace BubblePuzzle.Puzzles
{
    [CustomEditor(typeof(PuzzleController))]
    public class PuzzleControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(10);
            
            var puzzleController = (PuzzleController)target;
            
            using (new EditorGUILayout.HorizontalScope())
            {
                GUI.enabled = puzzleController.Puzzle != null;
                if (GUILayout.Button("Save"))
                {
                    puzzleController.SaveToConfiguration();
                }
            }
            
            if (!GUI.enabled)
            {
                EditorGUILayout.HelpBox("First set a puzzle configuration.", MessageType.Error);
            }
        }
    }
}