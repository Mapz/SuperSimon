using UnityEditor;
using UnityEngine;

namespace UnityEditor
{
    [CustomEditor(typeof(UnitSpawner))]
    public class SpawnerScene : Editor
    {

        void OnSceneGUI()
        {
            // get the chosen game object
            UnitSpawner t = target as UnitSpawner;
            Grid grid = t.GetComponentsInParent<Grid>(true)[0];
            if (t == null || grid == null || t.spawns == null)
                return;
            Handles.color = Color.red;
            int i = 0;
            var fontStyle = new GUIStyle();
            fontStyle.fontSize = 13;
         
            foreach (var p in t.spawns)
            {
                Vector3 wp;
                wp = grid.CellToLocal(p.position) + t.transform.position;
                var size = 8f;
                Handles.RectangleHandleCap(i, wp + grid.cellSize / 2, Quaternion.identity, size, EventType.Repaint);
                Handles.Label(wp + Vector3.up * size * 2, "生怪器:" + p.EnemeyToProduce.name, fontStyle);
                i++;
            }

        }

    }
}