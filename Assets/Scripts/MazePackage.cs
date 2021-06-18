
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

        public void ClearXWall() {
            Debug.Log("Destroyed " + posXWall);
            if (posXWall != null)
                Object.Destroy(posXWall);
        }

        public void ClearZWall() {
            if (posZWall != null)
                Object.Destroy(posZWall);
        }

        public void ClearAllWalls() {
            this.ClearXWall();
            this.ClearZWall();
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
            int xCells = maze.GetXCells(), zCells = maze.GetZCells();
            this.cells = new MazeCell[xCells, zCells];
            for (int x = 0; x < xCells; x++) {
                for (int z = 0; z < zCells; z++) {
                    AddCell(x, z);
                }
            }
            (GetMazeCellByIndex(4, 4)!).Value.ClearXWall();
        }

        public GameObject ReplicateXWall(Vector3 loc, Quaternion rotation) {
            return Object.Instantiate(posXWall, loc, rotation);
        }

        public GameObject ReplicateZWall(Vector3 loc, Quaternion rotation) {
            return Object.Instantiate(posZWall, loc, rotation);
        }

        public void UpdateWallType(GameObject wallObject) {
            posXWall = Object.Instantiate(wallObject);
            posZWall = Object.Instantiate(wallObject);
       }

        public void AddCell(int x, int z) {
            this.AddCell(x, z, true, true);
        }

        public void AddCell(int x, int z, bool haveXWall, bool haveZWall) {
            if (validateXZ(x, z)) {
                Vector3 min = maze.GetBorder().min;
                float nX = Mathf.FloorToInt((x + min.x) * maze.GetCellXSize()),
                      nZ = Mathf.FloorToInt((z + min.z) * maze.GetCellZSize());
                Vector3 loc = new Vector3(nX, maze.GetStarting().y, nZ);
                MazeCell mazeWall = new MazeCell((haveXWall) ? ReplicateXWall(loc, new Quaternion()) : null, (haveZWall) ? ReplicateZWall(loc, Quaternion.Euler(0, 90, 0)) : null);
                cells.SetValue(mazeWall, x, z);
            } else {
                Debug.Log("Failed at index " + x + ", " + z);
            }
        }

        public void RemoveCell(int x, int z) {
            MazeCell? cell = GetMazeCellByIndex(x, z);
            if (cell != null) {
                (cell!).Value.ClearAllWalls();
                cells.SetValue(null, x, z);
            }
        }

        public MazeCell? GetMazeCellByCoordinates(float x, float z) {
            Vector3 min = maze.GetBorder().min;
            return GetMazeCellByIndex(Mathf.FloorToInt((x - min.x) / maze.GetCellXSize()), Mathf.FloorToInt((z - min.z) / maze.GetCellZSize()));
        }

        public MazeCell? GetMazeCellByIndex(int x, int z) {
            return validateXZ(x, z) ? cells[x, z] : (MazeCell?) null;
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