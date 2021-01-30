using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class ZoneGenerator : MonoBehaviour
{
    private List<Tile> TilePrefabs;
    private Dictionary<CELL_PLACEMENT, Vector3> CellPositions = new Dictionary<CELL_PLACEMENT, Vector3>();
    private Dictionary<CELL_PLACEMENT, Tile> InstancedTiles = new Dictionary<CELL_PLACEMENT, Tile>();

    // Start is called before the first frame update
    void Start()
    {
        TilePrefabs = Resources.LoadAll<Tile>("Tiles/").ToList();

        var grid = GetComponent<Grid>();
        CellPositions.Add(CELL_PLACEMENT.TOPLEFT, grid.CellToLocalInterpolated(new Vector3(-6.5f, 6.5f, 0)));
        CellPositions.Add(CELL_PLACEMENT.TOPRIGHT, grid.CellToLocalInterpolated(new Vector3(6.5f, 6.5f, 0)));
        CellPositions.Add(CELL_PLACEMENT.BOTLEFT, grid.CellToLocalInterpolated(new Vector3(-6.5f, -6.5f, 0)));
        CellPositions.Add(CELL_PLACEMENT.BOTRIGHT, grid.CellToLocalInterpolated(new Vector3(6.5f, -6.5f, 0)));

        GenerateZone(Tile.DIRECTION.WEST);
    }

    void GenerateZone(Tile.DIRECTION startPos)
    {
        var startTile = TilePrefabs.Where(x => x.Exits.Count > 1 && x.Exits.Contains(startPos)).Random();

        var startTileObj = Instantiate(startTile, transform);
        PlaceTile(startPos, startTileObj, true);

    }

    void PlaceTile(Tile.DIRECTION entrance, Tile tile, bool startSquare)
    {
        var exits = tile.Exits.Where(x => x != entrance);

        if (startSquare)
        {
            if (entrance == Tile.DIRECTION.WEST)
            {
                if (exits.Contains(Tile.DIRECTION.NORTH) && !exits.Contains(Tile.DIRECTION.SOUTH))
                {
                    //BOTLEFT
                    tile.transform.position = CellPositions[CELL_PLACEMENT.BOTLEFT];
                    InstancedTiles.Add(CELL_PLACEMENT.BOTLEFT, tile);
                    return;
                }
                if (exits.Contains(Tile.DIRECTION.SOUTH) && !exits.Contains(Tile.DIRECTION.NORTH))
                {
                    //TOPLEFT
                    tile.transform.position = CellPositions[CELL_PLACEMENT.TOPLEFT];
                    InstancedTiles.Add(CELL_PLACEMENT.TOPLEFT, tile);
                    return;
                }
                var placements = new List<CELL_PLACEMENT> { CELL_PLACEMENT.BOTLEFT, CELL_PLACEMENT.TOPLEFT };
                var placement = placements.Random();
                tile.transform.position = CellPositions[placement];
                InstancedTiles.Add(placement, tile);
            }
            else if (entrance == Tile.DIRECTION.EAST)
            {
                if (exits.Contains(Tile.DIRECTION.NORTH) && !exits.Contains(Tile.DIRECTION.SOUTH))
                {
                    //BOTRIGHT
                    tile.transform.position = CellPositions[CELL_PLACEMENT.BOTRIGHT];
                    InstancedTiles.Add(CELL_PLACEMENT.BOTRIGHT, tile);
                    return;
                }
                if (exits.Contains(Tile.DIRECTION.SOUTH) && !exits.Contains(Tile.DIRECTION.NORTH))
                {
                    //TOPRIGHT
                    tile.transform.position = CellPositions[CELL_PLACEMENT.TOPRIGHT];
                    InstancedTiles.Add(CELL_PLACEMENT.TOPRIGHT, tile);
                    return;
                }
                var placements = new List<CELL_PLACEMENT> { CELL_PLACEMENT.BOTRIGHT, CELL_PLACEMENT.TOPRIGHT };
                var placement = placements.Random();
                tile.transform.position = CellPositions[placement];
                InstancedTiles.Add(placement, tile);
            }
            else if (entrance == Tile.DIRECTION.SOUTH)
            {
                if (exits.Contains(Tile.DIRECTION.WEST) && !exits.Contains(Tile.DIRECTION.EAST))
                {
                    //BOTRIGHT
                    tile.transform.position = CellPositions[CELL_PLACEMENT.BOTRIGHT];
                    InstancedTiles.Add(CELL_PLACEMENT.BOTRIGHT, tile);
                    return;
                }
                if (exits.Contains(Tile.DIRECTION.EAST) && !exits.Contains(Tile.DIRECTION.WEST))
                {
                    //BOTLEFT
                    tile.transform.position = CellPositions[CELL_PLACEMENT.BOTLEFT];
                    InstancedTiles.Add(CELL_PLACEMENT.BOTLEFT, tile);
                    return;
                }
                var placements = new List<CELL_PLACEMENT> { CELL_PLACEMENT.BOTRIGHT, CELL_PLACEMENT.BOTLEFT };
                var placement = placements.Random();
                tile.transform.position = CellPositions[placement];
                InstancedTiles.Add(placement, tile);
            }
            else if (entrance == Tile.DIRECTION.NORTH)
            {
                if (exits.Contains(Tile.DIRECTION.WEST) && !exits.Contains(Tile.DIRECTION.EAST))
                {
                    //TOPRIGHT
                    tile.transform.position = CellPositions[CELL_PLACEMENT.TOPRIGHT];
                    InstancedTiles.Add(CELL_PLACEMENT.TOPRIGHT, tile);
                    return;
                }
                if (exits.Contains(Tile.DIRECTION.EAST) && !exits.Contains(Tile.DIRECTION.WEST))
                {
                    //TOPLEFT
                    tile.transform.position = CellPositions[CELL_PLACEMENT.TOPLEFT];
                    InstancedTiles.Add(CELL_PLACEMENT.TOPLEFT, tile);
                    return;
                }
                var placements = new List<CELL_PLACEMENT> { CELL_PLACEMENT.TOPLEFT, CELL_PLACEMENT.TOPRIGHT };
                var placement = placements.Random();
                tile.transform.position = CellPositions[placement];
                InstancedTiles.Add(placement, tile);
            }
        }
        else
        {

        }
    }

    public enum CELL_PLACEMENT{
        TOPLEFT,
        TOPRIGHT,
        BOTLEFT,
        BOTRIGHT
    }
}
