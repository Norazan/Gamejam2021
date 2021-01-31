using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using System;
using Random = UnityEngine.Random;
using static Utility.Helper;

public class ZoneGenerator : MonoBehaviour
{
    public GameObject player;

    private List<Tile> TilePrefabs;
    private Dictionary<CELL_PLACEMENT, Vector3> CellPositions = new Dictionary<CELL_PLACEMENT, Vector3>();
    private Dictionary<CELL_PLACEMENT, Tile> InstancedTiles = new Dictionary<CELL_PLACEMENT, Tile>();

    void Start()
    {
        TilePrefabs = Resources.LoadAll<Tile>("Tiles/").ToList();

        var grid = GetComponent<Grid>();
        CellPositions.Add(CELL_PLACEMENT.TOPLEFT, grid.CellToLocalInterpolated(new Vector3(-6.5f, 6.5f, 0)));
        CellPositions.Add(CELL_PLACEMENT.TOPRIGHT, grid.CellToLocalInterpolated(new Vector3(6.5f, 6.5f, 0)));
        CellPositions.Add(CELL_PLACEMENT.BOTLEFT, grid.CellToLocalInterpolated(new Vector3(-6.5f, -6.5f, 0)));
        CellPositions.Add(CELL_PLACEMENT.BOTRIGHT, grid.CellToLocalInterpolated(new Vector3(6.5f, -6.5f, 0)));

        GenerateZone();
    }

    public void Clear()
    {
        foreach(var tile in InstancedTiles)
        {
            Destroy(tile.Value.gameObject);
        }
        InstancedTiles.Clear();
    }

    public void GenerateZone()
    {
        // Stage 1: Randomly choose tiles with a chance of creating a center wall, and place them in the proper cells.
        if (Random.value <= 0.5 && TilePrefabs.Any(x => x.blockades.Count > 0))
        {
            var blockadeTile = TilePrefabs.Where(x => x.blockades.Count > 0).Random();
            var blockade_direction = blockadeTile.blockades.First();

            PlaceTile(blockadeTile, RandomInnerCellPlacement(blockade_direction));

            //if (blockade_direction == Tile.DIRECTION.WEST)
            //{
            //    CELL_PLACEMENT[] placements = { CELL_PLACEMENT.TOPRIGHT, CELL_PLACEMENT.BOTRIGHT };
            //    CELL_PLACEMENT placement = placements.Random();

            //    PlaceTile(blockadeTile, placement);
            //}
            //else if (blockade_direction == Tile.DIRECTION.SOUTH)
            //{
            //    CELL_PLACEMENT[] placements = { CELL_PLACEMENT.TOPLEFT, CELL_PLACEMENT.TOPRIGHT };
            //    CELL_PLACEMENT placement = placements.Random();

            //    PlaceTile(blockadeTile, placement);
            //}
            //else if (blockade_direction == Tile.DIRECTION.EAST)
            //{
            //    CELL_PLACEMENT[] placements = { CELL_PLACEMENT.TOPLEFT, CELL_PLACEMENT.BOTLEFT };
            //    CELL_PLACEMENT placement = placements.Random();

            //    PlaceTile(blockadeTile, placement);
            //}
            //else if (blockade_direction == Tile.DIRECTION.NORTH)
            //{
            //    CELL_PLACEMENT[] placements = { CELL_PLACEMENT.BOTLEFT, CELL_PLACEMENT.BOTRIGHT };
            //    CELL_PLACEMENT placement = placements.Random();

            //    PlaceTile(blockadeTile, placement);
            //}

            var otherTiles = TilePrefabs.Where(x => x.blockades.Count == 0).RandomMany(3);
            var otherPlacements = GetValues<CELL_PLACEMENT>().Where(x => x != InstancedTiles.First().Key).ToList();
            otherPlacements.Shuffle();

            var tilesAndPlacements = otherTiles.Combine(otherPlacements);

            foreach (var pair in tilesAndPlacements)
            {
                PlaceTile(pair.Item1, pair.Item2);
            }
        }
        else
        {
            var tiles = TilePrefabs.Where(x => x.blockades.Count == 0).RandomMany(4);
            var placements = GetValues<CELL_PLACEMENT>().ToList();
            placements.Shuffle();

            var tilesAndPlacements = tiles.Combine(placements);

            foreach (var pair in tilesAndPlacements)
            {
                PlaceTile(pair.Item1, pair.Item2);
            }
        }

        // Generate walls
        InstancedTiles[CELL_PLACEMENT.BOTLEFT].WestWall.SetActive(true);
        InstancedTiles[CELL_PLACEMENT.BOTLEFT].SouthWall.SetActive(true);

        InstancedTiles[CELL_PLACEMENT.TOPLEFT].WestWall.SetActive(true);
        InstancedTiles[CELL_PLACEMENT.TOPLEFT].NorthWall.SetActive(true);

        InstancedTiles[CELL_PLACEMENT.TOPRIGHT].EastWall.SetActive(true);
        InstancedTiles[CELL_PLACEMENT.TOPRIGHT].NorthWall.SetActive(true);

        InstancedTiles[CELL_PLACEMENT.BOTRIGHT].EastWall.SetActive(true);
        InstancedTiles[CELL_PLACEMENT.BOTRIGHT].SouthWall.SetActive(true);

        var startDirection = GetValues<Tile.DIRECTION>().Random();
        var endDirection = GetValues<Tile.DIRECTION>().Where(x => x != startDirection).Random();

        var StartTile = InstancedTiles[RandomEdgeCellPlacement(startDirection)];
        var StartWall = StartTile.WallFromDirection(startDirection);
        StartWall.SetActive(false);
        player.transform.position = StartWall.transform.position;

        var EndTile = InstancedTiles[RandomEdgeCellPlacement(endDirection)];
        EndTile.WallFromDirection(endDirection).SetActive(false);

    }

    void PlaceTile(Tile tilePrefab, CELL_PLACEMENT placement)
    {
        var instancedTile = Instantiate(tilePrefab, transform);
        instancedTile.transform.position = CellPositions[placement];

        InstancedTiles[placement] = instancedTile;
    }

    CELL_PLACEMENT RandomEdgeCellPlacement(Tile.DIRECTION dir)
    {
        if (dir == Tile.DIRECTION.WEST)
        {
            CELL_PLACEMENT[] placements = { CELL_PLACEMENT.TOPLEFT, CELL_PLACEMENT.BOTLEFT };
            return placements.Random();
        }
        else if (dir == Tile.DIRECTION.SOUTH)
        {
            CELL_PLACEMENT[] placements = { CELL_PLACEMENT.BOTLEFT, CELL_PLACEMENT.BOTRIGHT };
            return placements.Random();
        }
        else if (dir == Tile.DIRECTION.EAST)
        {
            CELL_PLACEMENT[] placements = { CELL_PLACEMENT.TOPRIGHT, CELL_PLACEMENT.BOTRIGHT };
            return placements.Random();
        }
        else
        {
            CELL_PLACEMENT[] placements = { CELL_PLACEMENT.TOPLEFT, CELL_PLACEMENT.TOPRIGHT };
            return placements.Random();
        }
    }

    CELL_PLACEMENT RandomInnerCellPlacement(Tile.DIRECTION dir)
    {
        if (dir == Tile.DIRECTION.WEST)
        {
            CELL_PLACEMENT[] placements = { CELL_PLACEMENT.TOPRIGHT, CELL_PLACEMENT.BOTRIGHT };
            return placements.Random();
        }
        else if (dir == Tile.DIRECTION.SOUTH)
        {
            CELL_PLACEMENT[] placements = { CELL_PLACEMENT.TOPLEFT, CELL_PLACEMENT.TOPRIGHT };
            return placements.Random();
        }
        else if (dir == Tile.DIRECTION.EAST)
        {
            CELL_PLACEMENT[] placements = { CELL_PLACEMENT.TOPLEFT, CELL_PLACEMENT.BOTLEFT };
            return placements.Random();
        }
        else
        {
            CELL_PLACEMENT[] placements = { CELL_PLACEMENT.BOTLEFT, CELL_PLACEMENT.BOTRIGHT };
            return placements.Random();
        }
    }

    public enum CELL_PLACEMENT{
        TOPLEFT,
        TOPRIGHT,
        BOTLEFT,
        BOTRIGHT
    }
}
