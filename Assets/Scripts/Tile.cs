using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isNorthExit;
    public bool isEastExit;
    public bool isSouthExit;
    public bool isWestExit;

    public GameObject North;
    public GameObject East;
    public GameObject South;
    public GameObject West;

    private HashSet<DIRECTION> exits = new HashSet<DIRECTION>();
    public HashSet<DIRECTION> Exits
    {
        get
        {
            if (exits.Count == 0)
            {
                if (isNorthExit)
                {
                    exits.Add(DIRECTION.NORTH);
                }
                if (isEastExit)
                {
                    exits.Add(DIRECTION.EAST);
                }
                if (isSouthExit)
                {
                    exits.Add(DIRECTION.SOUTH);
                }
                if (isWestExit)
                {
                    exits.Add(DIRECTION.WEST);
                }
            }

            return exits;
        }
    }
        
    // Start is called before the first frame update
    void Start()
    {
        North.SetActive(!isNorthExit);
        West.SetActive(!isWestExit);
        South.SetActive(!isSouthExit);
        East.SetActive(!isEastExit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enum DIRECTION
    {
        NORTH,
        EAST,
        SOUTH,
        WEST
    }
}
