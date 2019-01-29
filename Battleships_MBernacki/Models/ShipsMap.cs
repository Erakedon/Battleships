using Battleships_MBernacki.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships_MBernacki.Models
{
    public class ShipsMap : IShipsMap
    {
        public short[][] Map { get; set; }// 0 - blank | 1 - ship | -1 - miss | -2 - ship drowned
        public short MapSize { get; set; }
        public short[] ShipList { get; set; }
        
        public ShipsMap(short[][] map, short mapSize, short[] shipList)
        {
            Map = map;
            MapSize = mapSize;
            ShipList = shipList;
        }


        private bool[][] SearchedCells { get; set; }
        private short[] ShipsToFound { get; set; }

        private List<int[]> ShipPartsLeft { get; set; }


        public bool ValidateMap()
        {
            if (Map.Length != MapSize) return false;
            for (int i = 0; i < Map.Length; i++)
                if (Map[i].Length != MapSize) return false;

            SearchedCells = new bool[MapSize][];
            for (int i = 0; i < MapSize; i++)
            {
                SearchedCells[i] = new bool[MapSize];
                for (int j = 0; j < MapSize; j++)
                    SearchedCells[i][j] = false;
            }


            ShipPartsLeft = new List<int[]>();

            ShipsToFound = ShipList;


            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    if (SearchedCells[i][j]) continue;
                    SearchedCells[i][j] = true;
                    if (Map[i][j] == 0) continue;
                    else if(Map[i][j] == 1)
                    {
                        if (!CheckSlants(i, j)) return false;

                        int sxNeg = FindInDirection(i - 1, j, -1, 0);
                        int sxPos = FindInDirection(i + 1, j, 1, 0);

                        int syNeg = FindInDirection(i, j - 1, 0, -1);
                        int syPos = FindInDirection(i, j + 1, 0, 1);
                        
                        if (sxNeg + sxPos > 0)
                        {
                            ShipsToFound[sxNeg + sxPos] -= 1;
                            int x = i + sxPos;
                            while (x >= i - sxNeg)
                            {
                                if (!CheckSlants(x, j)) return false;
                                //ShipsLeft[sxNeg + sxPos].Add(new int[2] { x, j });
                                ShipPartsLeft.Add(new int[] { x, j });
                                x--;
                            }
                            break;
                        }
                        else if (syNeg + syPos > 0)
                        {
                            ShipsToFound[syNeg + syPos] -= 1;
                            int y = j + syPos;
                            while (y >= j - syNeg)
                            {
                                if (!CheckSlants(i, y)) return false;
                                //ShipsLeft[sxNeg + sxPos].Add(new int[2] { i, y });
                                ShipPartsLeft.Add(new int[] { i, y });
                                y--;
                            }
                            break;
                        }
                    }
                }
            }




            return true;
        }

        private bool CheckSlants(int x,int y)
        {
            if (x > 0)
            {
                if (y > 0)
                {
                    if (Map[x - 1][y - 1] == 1) return false;
                    SearchedCells[x - 1][y - 1] = true;
                }
                if (y + 1 < MapSize)
                {
                    if(Map[x - 1][y + 1] == 1) return false;
                    SearchedCells[x - 1][y + 1] = true;
                }
            }
            if (x + 1 < MapSize)
            {
                if (y > 0)
                {
                    if(Map[x + 1][y - 1] == 1) return false;
                    SearchedCells[x + 1][y - 1] = true;
                }
                if (y + 1 < MapSize)
                {
                    if(Map[x + 1][y + 1] == 1) return false;
                    SearchedCells[x + 1][y + 1] = true;
                }
            }
            return true;
        }

        private int FindInDirection(int x, int y, int dirX, int dirY)
        {
            if (x < 0 || x + 1 > MapSize || y < 0 || y + 1 > MapSize) return 0;

            if (Map[x][y] == 1)
            {
                SearchedCells[x][y] = true;

                return 1 + FindInDirection(x + dirX, y + dirY, dirX, dirY);
            }
            SearchedCells[x][y] = true;
            return 0;


        }



        public string Shoot(int x, int y)
        {
            bool hit = false;

            int[] pointToRemove = new int[] { x, y };

            ShipPartsLeft.ForEach( s => {
                if (s[0] == x && s[1] == y)
                {
                    hit = true;
                    pointToRemove = s;
                }
            });

            if (hit) ShipPartsLeft.Remove(pointToRemove);

            if(ShipPartsLeft.Count() == 0) return "win";
            if (hit) return "hit";
            return "miss";
        }





    }
}
