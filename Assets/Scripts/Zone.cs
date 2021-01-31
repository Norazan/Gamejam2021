using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;
using static Utility.Helper;
using Random = UnityEngine.Random;
using DIRECTION = Tile.DIRECTION;

public class Zone : MonoBehaviour
{
    public Dictionary<CELL_PLACEMENT, Tile> InstancedTiles = new Dictionary<CELL_PLACEMENT, Tile>();
    private Tuple<CELL_PLACEMENT, DIRECTION> StartPosition;

    public Vector3 GetStartPosition()
    {
        return InstancedTiles[StartPosition.Item1].WallFromDirection(StartPosition.Item2).transform.position;
    }

    public void Populate(List<Tile> tilePrefabs, Tuple<CELL_PLACEMENT, DIRECTION> startPosition)
    {
        // Stage 1: Randomly choose tiles with a chance of creating a center wall, and place them in the proper cells.
        if (Random.value <= 0.5 && tilePrefabs.Any(x => x.blockades.Count > 0))
        {
            var blockadeTile = tilePrefabs.Where(x => x.blockades.Count > 0).Random();
            var blockade_direction = blockadeTile.blockades.First();

            PlaceTile(blockadeTile, RandomInnerCellPlacement(blockade_direction));

            var otherTiles = tilePrefabs.Where(x => x.blockades.Count == 0).RandomMany(3);
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
            var tiles = tilePrefabs.Where(x => x.blockades.Count == 0).RandomMany(4);
            var placements = GetValues<CELL_PLACEMENT>().ToList();
            placements.Shuffle();

            var tilesAndPlacements = tiles.Combine(placements);

            foreach (var pair in tilesAndPlacements)
            {
                PlaceTile(pair.Item1, pair.Item2);
            }
        }

        // Stage 2: Generate walls
        InstancedTiles[CELL_PLACEMENT.BOTLEFT].WestWall.SetActive(true);
        InstancedTiles[CELL_PLACEMENT.BOTLEFT].SouthWall.SetActive(true);

        InstancedTiles[CELL_PLACEMENT.TOPLEFT].WestWall.SetActive(true);
        InstancedTiles[CELL_PLACEMENT.TOPLEFT].NorthWall.SetActive(true);

        InstancedTiles[CELL_PLACEMENT.TOPRIGHT].EastWall.SetActive(true);
        InstancedTiles[CELL_PLACEMENT.TOPRIGHT].NorthWall.SetActive(true);

        InstancedTiles[CELL_PLACEMENT.BOTRIGHT].EastWall.SetActive(true);
        InstancedTiles[CELL_PLACEMENT.BOTRIGHT].SouthWall.SetActive(true);

        var StartCellPlacement = startPosition.Item1;
        var StartDirection = startPosition.Item2;
        var StartTile = InstancedTiles[StartCellPlacement];
        var StartWall = StartTile.WallFromDirection(StartDirection);
        StartWall.SetActive(false);
        StartPosition = new Tuple<CELL_PLACEMENT, DIRECTION>(StartCellPlacement, StartDirection);

        var zonePosition = ZoneGenerator.instance.GetCoord(this);
        var EndDirection = GetValues<DIRECTION>().Where(
            x => x != StartDirection && 
            !ZoneGenerator.instance.ZoneAlreadyExists(NewMapPosition(zonePosition, x))
        ).Random();
        var EndTile = InstancedTiles[RandomEdgeCellPlacement(EndDirection)];
        var EndWall = EndTile.WallFromDirection(EndDirection);
        foreach (Transform child in EndWall.transform)
        {
            child.gameObject.SetActive(false);
        }
        EndWall.AddComponent<Exit>();
    }

    void PlaceTile(Tile tilePrefab, CELL_PLACEMENT placement)
    {
        var instancedTile = Instantiate(tilePrefab, transform);
        instancedTile.transform.localPosition = GetCellPosition(placement);

        InstancedTiles[placement] = instancedTile;
    }

    Vector3 GetCellPosition(CELL_PLACEMENT placement)
    {
        var grid = GetComponent<Grid>();
        if (placement == CELL_PLACEMENT.TOPLEFT)
        {
            return grid.CellToLocalInterpolated(new Vector3(-6.5f, 6.5f, 0));
        }
        else if (placement == CELL_PLACEMENT.TOPRIGHT)
        {
            return grid.CellToLocalInterpolated(new Vector3(6.5f, 6.5f, 0));
        }
        else if (placement == CELL_PLACEMENT.BOTLEFT)
        {
            return grid.CellToLocalInterpolated(new Vector3(-6.5f, -6.5f, 0));
        }
        else
        {
            return grid.CellToLocalInterpolated(new Vector3(6.5f, -6.5f, 0));
        }
    }

    public enum CELL_PLACEMENT
    {
        TOPLEFT,
        TOPRIGHT,
        BOTLEFT,
        BOTRIGHT
    }
}
