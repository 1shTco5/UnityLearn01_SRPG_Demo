using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CanEditMultipleObjects, CustomEditor(typeof(Enemy))]
public class EnemyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Set Position"))
        {
            Tilemap tilemap = GameObject.Find("Grid/ground").GetComponent<Tilemap>();

            var allPos = tilemap.cellBounds.allPositionsWithin;

            int minX = 0;
            int minY = 0;

            if (allPos.MoveNext())
            {
                Vector3Int curr = allPos.Current;
                minX = curr.x;
                minY = curr.y;
            }

            Enemy enemy = target as Enemy;

            Vector3Int cellPos = tilemap.WorldToCell(enemy.transform.position);

            enemy.rowIndex = Mathf.Abs(minY - cellPos.y);
            enemy.colIndex = Mathf.Abs(minX - cellPos.x);

            enemy.transform.position = tilemap.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f, -1);
        }
    }
}
