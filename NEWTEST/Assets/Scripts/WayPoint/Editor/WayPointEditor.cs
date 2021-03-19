using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WayPoint))]
public class WayPointEditor : Editor
{
    WayPoint waypoint => target as WayPoint;
    private void OnSceneGUI()
    {
        Handles.color = Color.red;
        for (int i = 0; i < waypoint.Points.Length; i++)
        {
            EditorGUI.BeginChangeCheck();

            //Create Handles
            Vector3 currentWaypointPoint = waypoint.CurrentPosition + waypoint.Points[i];
            Vector3 newWaypointPoint = Handles.FreeMoveHandle(currentWaypointPoint, Quaternion.identity, .7f, new Vector3(.3f, .3f, .3f), Handles.SphereHandleCap);
            //Create Text
            GUIStyle textStyle = new GUIStyle();
            textStyle.fontStyle = FontStyle.Bold;
            textStyle.fontSize = 16;
            textStyle.normal.textColor = Color.yellow;
            Vector3 textAlligment = Vector3.down * .35f + Vector3.right * .35f;
            Handles.Label(waypoint.CurrentPosition + waypoint.Points[i] + textAlligment, $"{i + 1}", textStyle);

            EditorGUI.EndChangeCheck();

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Free Move Handle");
                waypoint.Points[i] = newWaypointPoint - waypoint.CurrentPosition;
            }
        }
    }

}
