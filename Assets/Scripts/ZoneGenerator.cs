using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using CELL_PLACEMENT = Zone.CELL_PLACEMENT;
using DIRECTION = Tile.DIRECTION;
using Utility;
using static Utility.Helper;

public class ZoneGenerator : MonoBehaviour
{
    public static ZoneGenerator instance;

    public GameObject player;
    public Zone zonePrefab;

    private List<Tile> TilePrefabs;
    private Dictionary<Vector2Int, Zone> WorldMap = new Dictionary<Vector2Int, Zone>();

    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        TilePrefabs = Resources.LoadAll<Tile>("Tiles/").ToList();

        GenerateZone(setPlayerPosition: true);
    }

    public void GenerateZone(Vector2Int zonePos = default(Vector2Int), Tuple<CELL_PLACEMENT, DIRECTION> startPos = null, bool setPlayerPosition = false)
    {
        if(startPos == null)
        {
            var randomDirection = GetValues<DIRECTION>().Random();
            startPos = new Tuple<CELL_PLACEMENT, DIRECTION>(RandomEdgeCellPlacement(randomDirection), randomDirection);
        }

        // Stage 0: Instantiate zone
        var currentZone = Instantiate(zonePrefab);
        currentZone.transform.position = new Vector3(zonePos.x * 20.48f, zonePos.y * 20.48f, 0);
        WorldMap.Add(zonePos, currentZone);
        currentZone.Populate(TilePrefabs, startPos);
        if (setPlayerPosition)
        {
            player.transform.position = currentZone.GetStartPosition();
        }
        player.GetComponent<PlayerMovement>().currentZone = zonePos;
    }

    public Zone GetZone(Vector2Int coord)
    {
        return WorldMap[coord];
    }

    public Vector2Int GetCoord(Zone zone)
    {
        return WorldMap.First(x => x.Value == zone).Key;
    }

    public bool ZoneAlreadyExists(Vector2Int pos)
    {
        return WorldMap.ContainsKey(pos);
    }

    //public void Clear()
    //{
    //    foreach(var tile in InstancedTiles)
    //    {
    //        Destroy(tile.Value.gameObject);
    //    }
    //    InstancedTiles.Clear();
    //}
}
