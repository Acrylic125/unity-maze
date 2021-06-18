
using System;
using System.Collections.Generic;

namespace MazePackage {

    public class HuntKillMazeAlgorithm<T> 
            : MazeAlgorithm<T> where T : IMaze
    {

        public delegate void HunterDelegate(int directionX, int directionZ, MazeWall? wall, MazeCell? cellOfWall);

        public struct HunterCell {
            public readonly MazeCell cell;
            public readonly int facingX, facingZ;

            public HunterCell(MazeCell cell, int facingX, int facingZ) {
                this.cell = cell;
                this.facingX = facingX;
                this.facingZ = facingZ;
            }

        }

        private Stack<HunterCell> open;
        private LinkedList<HunterCell> closed;

        public HuntKillMazeAlgorithm(T maze): base(maze) {}

        public override void build() {
            
            open = new Stack<HunterCell>();
            closed = new LinkedList<HunterCell>();
            MazeMap map = maze.GetMazeMap();
        }

        private void iterateWallsSurroundingCell(MazeCell cell, HunterDelegate action) {
            MazeMap map = maze.GetMazeMap();
            int x = cell.x, z = cell.z;
            action(1, 0, cell.posXWall, cell);
            action(0, 1, cell.posZWall, cell);
            MazeCell? iCell = map.GetMazeCellByIndex(x - 1, z);
            action(-1, 0, (iCell == null) ? (MazeWall?) null : cell.posXWall, iCell);
            iCell = map.GetMazeCellByIndex(x, z - 1);
            action(0, -1, (iCell == null) ? (MazeWall?) null : cell.posZWall, iCell);
         }

        private bool isWallBlocked(MazeWall? wall) {
            return (wall == null) || wall.Value.blocked;
        }

        private List<MazeCell> getAccessible(int x, int z) {
            List<MazeCell> accessible = new List<MazeCell>();
            MazeMap map = maze.GetMazeMap();
            MazeCell? thisCell = map.GetMazeCellByIndex(x, z);
            if (thisCell != null) {
                MazeCell thisCellVal = thisCell.Value;
                iterateWallsSurroundingCell(thisCellVal, (dirX, dirZ, wall, cellOfWall) => {
                    if (cellOfWall != null && !isWallBlocked(cellOfWall.Value.posXWall)) {
                        
                    }
                });
            }
            return accessible;
        }

    }
}