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
            if (line.IsDrawableConnected(out Vector3 connectionPoint))
            {
                Debug.Log(line + " po��czony z rysunkiem w punkcie " + connectionPoint);
                string str = "";
                foreach(Vector3 point in line.GetConnectionPointsWith(CopperLine.Drawable))
                {
                    str += point + ", ";
                }

                Debug.Log("Punkty: " + str);
            }
            else
            {
                Debug.Log(line + " nie jest po��czony z rysunkiem");
            }
        }
    }
}
