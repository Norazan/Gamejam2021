using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public List<DIRECTION> blockades;

    public GameObject NorthWall;
    public GameObject EastWall;
    public GameObject SouthWall;
    public GameObject WestWall;

    //private HashSet<DIRECTION> exits = new HashSet<DIRECTION>();
    //public HashSet<DIRECTION> Exits
    //{
    //    get
    //    {
    //        if (exits.Count == 0)
    //        {
    //            if (isNorthExit)
    //            {
    //                exits.Add(DIRECTION.NORTH);
    //            }
    //            if (isEastExit)
    //            {
    //                exits.Add(DIRECTION.EAST);
    //            }
    //            if (isSouthExit)
    //            {
    //                exits.Add(DIRECTION.SOUTH);
    //            }
    //            if (isWestExit)
    //            {
    //                exits.Add(DIRECTION.WEST);
    //            }
    //        }

    //        return exits;
    //    }
    //}
        
    // Start is called before the first frame update
    void Start()
    {
        //North.SetActive(!isNorthExit);
        //West.SetActive(!isWestExit);
        //South.SetActive(!isSouthExit);
        //East.SetActive(!isEastExit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject WallFromDirection(DIRECTION dir)
    {
        if(dir == DIRECTION.NORTH)
        {
            return NorthWall;
        }
        else if(dir == DIRECTION.EAST)
        {
            return EastWall;
        }
        else if(dir == DIRECTION.SOUTH)
        {
            return SouthWall;
        }
        else
        {
            return WestWall;
        }
    }

    public DIRECTION DirectionFromWall(GameObject wallObj)
    {
        if (wallObj == NorthWall)
        {
            return DIRECTION.NORTH;
        }
        else if (wallObj == EastWall)
        {
            return DIRECTION.EAST;
        }
        else if (wallObj == SouthWall)
        {
            return DIRECTION.SOUTH;
        }
        else
        {
            return DIRECTION.WEST;
        }
    }

    public enum DIRECTION
    {
        NORTH,
        EAST,
        SOUTH,
        WEST
    }
}
