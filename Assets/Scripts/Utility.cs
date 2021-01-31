using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DIRECTION = Tile.DIRECTION;
using CELL_PLACEMENT = Zone.CELL_PLACEMENT;
using Random = System.Random;

namespace Utility
{
    public static class Extensions
    {
        private static readonly Random rng = new Random();

        public static IEnumerable<T> RandomMany<T>(this IEnumerable<T> enumerable, int number)
        {
            for(int _ = 0; _ < number; _++)
            {
                yield return enumerable.Random();
            }
        }

        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.ElementAt(rng.Next(enumerable.Count()));
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count();
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static IEnumerable<Tuple<TFirst, TSecond>> Combine<TFirst, TSecond>(this IEnumerable<TFirst> s1, IEnumerable<TSecond> s2)
        {
            using var e1 = s1.GetEnumerator();
            using var e2 = s2.GetEnumerator();
            while (e1.MoveNext() && e2.MoveNext())
            {
                yield return new Tuple<TFirst, TSecond>(e1.Current, e2.Current);
            }
        }

        public static DIRECTION Opposite(this DIRECTION dir)
        {
            if (dir == DIRECTION.NORTH)
            {
                return DIRECTION.SOUTH;
            }
            else if (dir == DIRECTION.EAST)
            {
                return DIRECTION.WEST;
            }
            else if (dir == DIRECTION.SOUTH)
            {
                return DIRECTION.NORTH;
            }
            else
            {
                return DIRECTION.EAST;
            }
        }

        public static CELL_PLACEMENT AttachedCell(this CELL_PLACEMENT placement, DIRECTION attachDirection)
        {
            if (attachDirection == DIRECTION.NORTH || attachDirection == DIRECTION.SOUTH)
            {
                if (placement == CELL_PLACEMENT.TOPLEFT)
                {
                    return CELL_PLACEMENT.BOTLEFT;
                }
                else if (placement == CELL_PLACEMENT.BOTLEFT)
                {
                    return CELL_PLACEMENT.TOPLEFT;
                }
                else if (placement == CELL_PLACEMENT.TOPRIGHT)
                {
                    return CELL_PLACEMENT.BOTRIGHT;
                }
                else
                {
                    return CELL_PLACEMENT.TOPRIGHT;
                }
            }
            else
            {
                if (placement == CELL_PLACEMENT.TOPLEFT)
                {
                    return CELL_PLACEMENT.TOPRIGHT;
                }
                else if (placement == CELL_PLACEMENT.BOTLEFT)
                {
                    return CELL_PLACEMENT.BOTRIGHT;
                }
                else if (placement == CELL_PLACEMENT.TOPRIGHT)
                {
                    return CELL_PLACEMENT.TOPLEFT;
                }
                else
                {
                    return CELL_PLACEMENT.BOTLEFT;
                }
            }
        }

        public static Tuple<CELL_PLACEMENT, DIRECTION> AttachedCell(this Tuple<CELL_PLACEMENT, DIRECTION> initCell)
        {
            return new Tuple<CELL_PLACEMENT, DIRECTION>(initCell.Item1.AttachedCell(initCell.Item2), initCell.Item2.Opposite());
        }
    }

    public static class Helper
    {
        public static IEnumerable<T> GetValues<T>() where T : Enum
        {
            return (T[])Enum.GetValues(typeof(T));
        }

        public static CELL_PLACEMENT RandomEdgeCellPlacement(DIRECTION dir)
        {
            if (dir == DIRECTION.WEST)
            {
                CELL_PLACEMENT[] placements = { CELL_PLACEMENT.TOPLEFT, CELL_PLACEMENT.BOTLEFT };
                return placements.Random();
            }
            else if (dir == DIRECTION.SOUTH)
            {
                CELL_PLACEMENT[] placements = { CELL_PLACEMENT.BOTLEFT, CELL_PLACEMENT.BOTRIGHT };
                return placements.Random();
            }
            else if (dir == DIRECTION.EAST)
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

        public static CELL_PLACEMENT RandomInnerCellPlacement(DIRECTION dir)
        {
            if (dir == DIRECTION.WEST)
            {
                CELL_PLACEMENT[] placements = { CELL_PLACEMENT.TOPRIGHT, CELL_PLACEMENT.BOTRIGHT };
                return placements.Random();
            }
            else if (dir == DIRECTION.SOUTH)
            {
                CELL_PLACEMENT[] placements = { CELL_PLACEMENT.TOPLEFT, CELL_PLACEMENT.TOPRIGHT };
                return placements.Random();
            }
            else if (dir == DIRECTION.EAST)
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

        public static Vector2Int NewMapPosition(Vector2Int oldPos, DIRECTION dir)
        {
            if (dir == DIRECTION.NORTH)
            {
                return oldPos + new Vector2Int(0, 1);
            }
            else if (dir == DIRECTION.EAST)
            {
                return oldPos + new Vector2Int(1, 0);
            }
            else if (dir == DIRECTION.SOUTH)
            {
                return oldPos + new Vector2Int(0, -1);
            }
            else
            {
                return oldPos + new Vector2Int(-1, 0);
            }
        }
    }
}
