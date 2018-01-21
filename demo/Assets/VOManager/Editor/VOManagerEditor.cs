using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// VOManagerEditor
/// Description: Custom editor for the VOManager.
/// </summary>

[CustomEditor(typeof(VOManager))]
public class VOManagerEditor : Editor
{
    private VOManager voManager;   // reference to the VOManager

    private void OnEnable()
    {
        // set the reference to the current inspected object
        voManager = (VOManager)target;
    }

    // overwrite inspector interface
    public override void OnInspectorGUI()
    {
        // record any changes done within the VOManager
        Undo.RecordObjects(targets, "VOManager");

        EditorGUILayout.Space();

        // if the text component is empty
        if (voManager.uiTextObject == null)
            // display a warning message
            EditorGUILayout.HelpBox("No UI Text component is found. Please set the UI Text Object.", MessageType.Warning);

        // text component field
        voManager.uiTextObject = (Text)EditorGUILayout.ObjectField("UI Text Object", voManager.uiTextObject, typeof(Text), true);

        EditorGUILayout.Space();
    }
}