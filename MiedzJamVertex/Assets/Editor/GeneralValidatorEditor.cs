using UnityEngine;
using UnityEditor;
using RoboMed.Puzzle;

[CustomEditor(typeof(GeneralValidator))]
public class GeneralValidatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GeneralValidator validator = (GeneralValidator)target;
        if (GUILayout.Button("Validate"))
        {
            if (validator.Validate())
            {
                Debug.Log("Wszystkie zagadki rozwi¹zano poprawnie. Wow!");
            }
            else
            {
                Debug.Log("Coœ chyba jest nie tak...");
            }
        }
    }
}
