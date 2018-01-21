using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// VOBankEditor
/// Description: Custom inspector editor for VOBank.
/// </summary>

[CustomEditor(typeof(VOBank))]
public class VOBankEditor : Editor
{
    // create a reference to the VOBank 
    private VOBank voBank;

    // on enabled
    private void OnEnable()
    {
        // set the reference to the current inspected object
        voBank = (VOBank)target;
    }

    // overwrite the inspector interface
    public override void OnInspectorGUI()
    {
        /// GUI STYLES

        // header styles
        GUIStyle styleRowHeader = new GUIStyle();
        styleRowHeader.padding = new RectOffset(0, 0, 3, 3);
        styleRowHeader.normal.background = EditorStyle.SetBackground(1, 1, new Color(0.1f, 0.1f, 0.1f, 0.2f));

        /// EDITOR 

        // record any changes done within the VOBank
        Undo.RecordObjects(targets, "VOBank");

        EditorGUILayout.Space();

        // info box to inform user if they have no clips in the sound bank
        if (voBank.bank.Count == 0)
            EditorGUILayout.HelpBox("You currently have no audio clips in your VO Bank. Press the Add Clip button below to add a clip.", MessageType.Info);
        else
            EditorGUILayout.HelpBox("Press the Add Clip button below to add more clips.", MessageType.Info);

        // for each clip inside the bank starting from the lowest to highest (order)
        for (int i = 0; i < voBank.bank.Count; i++)
        {
            // record any changes done within the VOBank bank list
            Undo.RecordObjects(targets, "ClipProperties");

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();

            // expand or collapse the clip's panel depending on the boolean status
            // and name the panel based on its id and clip name
            voBank.bank[i].blnExpandClipPnl = EditorGUILayout.Foldout(voBank.bank[i].blnExpandClipPnl, "[ID " + voBank.bank[i].id + "] " + voBank.bank[i].strName);

            // if the remove button is clicked on
            if (GUILayout.Button("-", GUILayout.MinWidth(25.0f), GUILayout.MaxWidth(25.0f)))
                // remove that current audio clip from the bank
                voBank.RemoveClip(i);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            // try and run the following
            try
            {
                // if the clip's panel is expanded
                if (voBank.bank[i].blnExpandClipPnl)
                {
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.BeginHorizontal(styleRowHeader);
                    EditorGUILayout.LabelField("Clip Properties", EditorStyles.boldLabel);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    EditorGUILayout.HelpBox("Keep in mind all IDs and Clip Names must be unique. Having duplicates in the VO Bank will cause errors.", MessageType.Info);
                    EditorGUILayout.Space();

                    // set the clip's id 
                    voBank.bank[i].id = EditorGUILayout.IntField("ID", voBank.bank[i].id);

                    // show the name field
                    voBank.bank[i].strName = EditorGUILayout.TextField("Clip Name", voBank.bank[i].strName);

                    // show the audio clip field
                    voBank.bank[i].audClp = (AudioClip)EditorGUILayout.ObjectField("VO Clip", voBank.bank[i].audClp, typeof(AudioClip), true);

                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal(styleRowHeader);

                    EditorGUILayout.LabelField("Subtitle", EditorStyles.boldLabel);


                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    
                    EditorGUILayout.BeginHorizontal();

                    voBank.bank[i].subtitle = EditorGUILayout.TextField(voBank.bank[i].subtitle);

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndVertical();
                }
            }
            catch
            {
                // throw a warning if there is more then one item in the bank
                if (voBank.bank.Count > 1)
                    Debug.LogError("There is an error reading the bank due to the custom editor.");

                return;
            }
        }

        // if the add clip button is pressed
        if (GUILayout.Button("Add Clip"))
            // add a clip to the bank
            voBank.AddEmptyClip();


        EditorGUILayout.Space();
    }
}
