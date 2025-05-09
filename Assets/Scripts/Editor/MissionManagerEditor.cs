#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom editor for the MissionManager component.
/// It adds debug controls to the inspector to control the current mission.
/// </summary>
[CustomEditor(typeof(MissionManager))]
public class MissionManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MissionManager missionManager = (MissionManager)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Debug Controls", EditorStyles.boldLabel);

        // Button to complete the current mission.
        if (GUILayout.Button("Complete Current Mission"))
        {
            missionManager.CompleteCurrentMission();
        }

        // Button to fail the current mission.
        if (GUILayout.Button("Fail Current Mission"))
        {
            missionManager.FailCurrentMission();
        }
    }
}
#endif