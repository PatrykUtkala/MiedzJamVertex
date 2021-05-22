using UnityEngine;
using UnityEditor;
using RoboMed.Drawing;

[CustomEditor(typeof(GraphVertex))]
public class GraphVertexEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GraphVertex vertex = (GraphVertex)target;
        if(GUILayout.Button("Po³¹cz z poprzednikiem"))
        {
            vertex.LinkToPrevious();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
