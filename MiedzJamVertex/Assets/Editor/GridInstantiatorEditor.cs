using UnityEngine;
using UnityEditor;
using RoboMed.CreatorUtilities;

[CustomEditor(typeof(GridInstantiator))]
public class GridInstantiatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridInstantiator gridInstantiator = (GridInstantiator)target;
        if(GUILayout.Button("Instantiate objects"))
        {
            gridInstantiator.InstantiateGrid();
        }
    }
}
