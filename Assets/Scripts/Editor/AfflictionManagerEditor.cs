#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom editor for the AfflictionManager class.
/// </summary>
[CustomEditor(typeof(AfflictionManager))]
public class AfflictionManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw default inspector fields.
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Affliction Testing", EditorStyles.boldLabel);

        // Buttons to queue afflictions.
        if (GUILayout.Button("Queue Drowniness"))
        {
            AfflictionManager.QueueAffliction(AfflictionType.Drowsiness);
        }
        if (GUILayout.Button("Queue Blurred Vision"))
        {
            AfflictionManager.QueueAffliction(AfflictionType.BlurredVision);
        }
        if (GUILayout.Button("Queue Fatigue"))
        {
            AfflictionManager.QueueAffliction(AfflictionType.Fatigue);
        }
    }
}
#endif