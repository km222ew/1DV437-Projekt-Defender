using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;


namespace Defender.Model
{
    class Level
    {
        public const int SIZE_X = 7;
        public const int SIZE_Y = 10;

        private Tile[,] tiles = new Tile[SIZE_X, SIZE_Y];
       
        public Level()
        { 
            
        }

        internal Tile[,] Tiles
        {
            get { return tiles; }
        }

        public Vector2 PlayerStartPosition()
        {
            return new Vector2(3, 8);
        }

        public bool LoadLevel(int currentLevel)
        {
            StreamReader levelReader;

            if (File.Exists(@"Content/Levels/Level" + currentLevel.ToString() + ".txt"))
            {           
                levelReader = new StreamReader(@"Content/Levels/Level" + currentLevel.ToString() + ".txt");

                string dirtyLevel = levelReader.ReadToEnd();

                levelReader.Close();

                string cleanLevel = dirtyLevel.Replace("\r\n", string.Empty);

                for (int x = 0; x < SIZE_X; x++)
			    {
                    for (int y = 0; y < SIZE_Y; y++)
                    {
                        int index = y * SIZE_X + x;

                        if (cleanLevel[index] == '0')
                        {
                            tiles[x, y] = Tile.createWall(x, y);
                        }
                        else if (cleanLevel[index] == '1')
                        {
                            tiles[x, y] = Tile.createFloor(x, y);
                        }
                        else if (cleanLevel[index] == '2')
                        {
                            tiles[x, y] = Tile.createBox(x, y);
                        }
                        else if (cleanLevel[index] == '3')
                        {
                            tiles[x, y] = Tile.createDrain(x, y);
                        }
                        else if (cleanLevel[index] == '4')
                        {
                            tiles[x, y] = Tile.createDanger(x, y);
                        }
                    }
			    }

                return true;

            }
            else
            {
                return false;
            }
        
        }


    }
}
