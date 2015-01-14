using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Defender.Model
{
    class Tile
    {
        public enum TileType
        {
            WALL = 0,
            FLOOR,
            BOX,
            DRAIN,
            DANGER

        };

        private TileType type;        
        private int leftX;
        private int topY;

        private Tile(TileType type, int x, int y)
        {
            this.type = type;
            this.leftX = x;
            this.topY = y;
        }

        internal TileType Type
        {
            get { return type; }
        }

        public static Tile createWall(int x, int y)
        {
            return new Tile(TileType.WALL, x, y);
        }

        public static Tile createFloor(int x, int y)
        {
            return new Tile(TileType.FLOOR, x, y);
        }

        public static Tile createBox(int x, int y)
        {
            return new Tile(TileType.BOX, x, y);
        }

        public static Tile createDrain(int x, int y)
        {
            return new Tile(TileType.DRAIN, x, y);
        }

        public static Tile createDanger(int x, int y)
        {
            return new Tile(TileType.DANGER, x, y);
        }
    }
}
