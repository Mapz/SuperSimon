using UnityEngine;
using UnityEditor;

public class ShowCheckPoint : Editor
{

    [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
    static void ShowCheckPoints(Transform transform, GizmoType gizmoType)
    {
        int i = 0;
        if (transform.GetComponent<CheckPointGO>()) {
            Handles.Label(transform.position, transform.gameObject.name);
            Handles.RectangleHandleCap(i, transform.position,Quaternion.identity,8,EventType.Repaint);
        }
    }
}