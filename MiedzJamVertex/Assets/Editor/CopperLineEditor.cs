using UnityEngine;
using UnityEditor;
using RoboMed.Drawing;

[CustomEditor(typeof(CopperLine))]
public class CopperLineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CopperLine line = (CopperLine)target;
        if(GUILayout.Button("Check drawn connected"))
        {
            if (line.IsDrawableConnected())
            {
                Debug.Log(line + " po³¹czony z rysunkiem");
            }
            else
            {
                Debug.Log(line + " nie jest po³¹czony z rysunkiem");
            }
        }
    }
}
