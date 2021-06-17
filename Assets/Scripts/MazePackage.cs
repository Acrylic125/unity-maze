
using UnityEngine;

namespace MazePackage
{

    public struct MazeCell {
        public GameObject posXWall, posZWall;

        public MazeCell(GameObject posXWall, GameObject posZWall)
        {
            this.posXWall = posXWall;
            this.posZWall = posZWall;
        }

        public bool hasXWall() {
            return posXWall != null;
        }

        public bool hasZWall() {
            return posZWall != null;
        }

        public void clearXWall() {
            if (posXWall != null)
                Object.Destroy(posXWall);
        }

        public void clearZWall() {
            if (posZWall != null)
                Object.Destroy(posZWall);
        }

        public void clearAllWalls() {
            this.clearXWall();
            this.clearZWall();
        }
    }

    public class MazeMap {

        public readonly IMaze maze;
        public MazeCell[,] cells;
        private GameObject posXWall, posZWall;

        public MazeMap(GameObject wallObject, IMaze maze) {
            UpdateWallType(wallObject);
            this.maze = maze;
            ReMap();
        }

        public void ReMap() {
            this.cells = new MazeCell[maze.GetXCells(), maze.GetZCells()];
        }

        public GameObject ReplicateXWall(Vector3 loc, Quaternion rotation) {
            return Object.Instantiate(posXWall, loc, rotation);
        }

        public GameObject ReplicateZWall(Vector3 loc, Quaternion rotation) {
            return Object.Instantiate(posZWall, loc, rotation);
        }

        public void UpdateWallType(GameObject wallObject) {
            
        }

        public void AddCell(int x, int z) {
            this.AddCell(x, z, true, true);
        }

        public void AddCell(int x, int z, bool haveXWall, bool haveZWall) {
            MazeCell mazeWall = new MazeCell((haveXWall) ? posXWall : null, (haveZWall) ? posZWall : null);
            cells.SetValue(mazeWall, x, z);
        }

        public void RemoveCell(int x, int z) {
            MazeCell? cell = GetMazeCellByIndex(x, z);
            if (cell != null) {
                (cell!).Value.clearAllWalls();
                cells.SetValue(null, x, z);
            }
        }

        public MazeCell? GetMazeCellByCoordinates(float x, float z) {
            Vector3 min = maze.GetBorder().min;
            return GetMazeCellByIndex(Mathf.FloorToInt((x - min.x) / maze.GetCellXSize()), Mathf.FloorToInt((z - min.z) / maze.GetCellZSize()));
        }

        public MazeCell? GetMazeCellByIndex(int x, int z) {
            return validateXZ(x, z) ? cells[x, z] : null;
        }

        protected bool validateXZ(int x, int z) {
            int s1 = cells.GetLength(0), s2 = cells.GetLength(1);
            return x >= 0 && z >= 0 && s1 > x && s2 > z;
        }
       
    }

    public interface IMaze {
        Bounds GetBorder();
        
        int GetXCells();
        
        int GetZCells();

        float GetXWallFactor();

        float GetZWallFactor();

        float GetCellXSize();

        float GetCellZSize();
        
        MazeMap GetMazeMap();

        Vector3 GetStarting();
    }

    public abstract class MazeAlgorithm<T>
                    where T : IMaze {
        
        public T maze;
        
        public MazeAlgorithm(T maze) {
            this.maze = maze;
        }

        public abstract void build();

    }

}