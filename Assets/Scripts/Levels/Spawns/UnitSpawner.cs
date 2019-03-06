using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SpawnData
{
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];

    public Vector3Int position;
    public GameObject EnemeyToProduce;
    //public string enemyName;
    public int maxCount;
    public float interval;
    public float rateOfSpawn;
    private int curCount = 0;
    private float curTime = 0;
    private bool active = false;

    public SpawnData(Vector3Int _position, GameObject _EnemeyToProduce, int _maxCount, float _interval)
    {
        position = _position;
        EnemeyToProduce = _EnemeyToProduce;
        maxCount = _maxCount;
        interval = _interval;
        curCount = 0;
        curTime = interval;
        active = false;
        rateOfSpawn = 1;
    }

    public SpawnData CloneSetPos(Vector3Int _position)
    {
        return new SpawnData(_position, EnemeyToProduce, maxCount, interval);
    }

    public void SetPosition(Vector3Int _position)
    {
        position = _position;
    }

    public void SetActive(bool _active)
    {
        active = _active;
    }

    public void UpdateSpawn(float delta, Grid grid, Transform parent, Transform spawnerTransform)
    {
        if (!active) return;
        if (curCount >= maxCount) return;
        curTime -= delta;
        if (curTime <= 0)
        {
            Spawn(grid, parent, spawnerTransform);
            curCount++;
            curTime = interval;
        }
    }

    public void Spawn(Grid grid, Transform parent, Transform spawnerTransform)
    {
        if (UnityEngine.Random.Range(0f, 1f) >= rateOfSpawn) return;
        GameObject enemy = ObjectMgr<Unit>.Instance.Create(() =>
        {
            return UnityEngine.Object.Instantiate(EnemeyToProduce).GetComponent<Unit>();
        }).gameObject;
        enemy.transform.parent = parent;
        enemy.transform.position = grid.CellToLocal(position) + spawnerTransform.position + grid.cellSize / 2;

        //var rb2d = enemy.GetComponent<Rigidbody2D>();
        //rb2d.MovePosition(enemy.transform.position);
        //var contactFilter = new ContactFilter2D();
        //contactFilter.useTriggers = false;
        //contactFilter.SetLayerMask(1 << LayerMask.NameToLayer("Ground"));
        //contactFilter.useLayerMask = true;
        //int count = rb2d.Cast(Vector2.down, contactFilter, hitBuffer);

        //for (int i = 0; i < count; i++)
        //{
        //    enemy.transform.position -= hitBuffer[i].distance * Vector3.down;
        //}
      

    }
}

public class UnitSpawner : MonoBehaviour
{

    [HideInInspector]
    public List<SpawnData> spawns;

    void Update()
    {
        if (!GameStatus.IsState<GameStateInGame>()) return;
        foreach (var data in spawns)
        {
            if (Utility.CheckInSpawnArea(transform.position + InGameVars.LevelConfigs.m_levelGrid.CellToLocal(data.position)))
            {
                Bounds spawnerBound = new Bounds(transform.position + InGameVars.LevelConfigs.m_levelGrid.CellToLocal(data.position), Vector3.one * 17);
                bool boundsIntersect = false;
                foreach (Unit u in ObjectMgr<Unit>.Instance)
                {
                    var collider = u.GetComponent<Collider2D>();
                    if (collider)
                    {
                        if (spawnerBound.Intersects(collider.bounds)) {
                            boundsIntersect = true;
                            break;
                        }
                    }
                }
                if (!boundsIntersect) {
                    data.SetActive(true);
                    data.UpdateSpawn(Time.deltaTime, InGameVars.LevelConfigs.m_levelGrid, InGameVars.LevelConfigs.m_EnemyObjectParentTransform, transform);
                }
            }
            else
            {
                data.SetActive(false);
            }

        }
    }

}