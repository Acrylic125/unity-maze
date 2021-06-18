
using UnityEngine;

namespace MazePackage
{

    public struct MazeWall {

        public GameObject wall;
        public bool border;
        
        public MazeWall(GameObject wall, bool border) {
            this.wall = wall;
            this.border = border;
        }

        public void Destroy() {
            if (wall != null)
                Object.Destroy(wall);
        }

    }

    public struct MazeCell {
        public MazeWall? posXWall, posZWall;

        public MazeCell(MazeWall? posXWall, MazeWall? posZWall)
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

        public void ClearAllWalls() {
            if (posXWall != null)
                (posXWall!).Value.Destroy();
            if (posZWall != null)
                (posZWall!).Value.Destroy();
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
            this.cells = new MazeCell[xCells + 1, zCells + 1];
            for (int x = 1; x < xCells; x++) {
                for (int z = 1; z < zCells; z++) {
                    AddCell(x, z);
                }
            }
            // Remap borders
            for (int x = 0; x < xCells; x++) {
                AddCell(x, 0, null, WallArgs.Border);
                AddCell(x, 0, WallArgs.Normal, null);
                AddCell(x, zCells, null, WallArgs.Border);
            }
            for (int z = 0; z < xCells; z++) {
                AddCell(0, z, WallArgs.Border, null);
                AddCell(0, z, null, WallArgs.Normal);
                AddCell(xCells, z, WallArgs.Border, null);
            }
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
            this.AddCell(x, z, WallArgs.Normal, WallArgs.Normal);
        }

        public struct WallArgs {
            
            public static readonly WallArgs Border = new WallArgs(true);
            public static readonly WallArgs Normal = new WallArgs(false);
            
            public readonly bool border;

            WallArgs(bool border) {
                this.border = border;
            }

        }

        public void AddCell(int x, int z, WallArgs? wargX, WallArgs? wargZ) {
            if (validateXZ(x, z)) {
                Vector3 min = maze.GetBorder().min;
                float nX = Mathf.FloorToInt((x + min.x) * maze.GetCellXSize()),
                      nZ = Mathf.FloorToInt((z + min.z) * maze.GetCellZSize()),
                      y = maze.GetStarting().y;
                Vector3 loc = new Vector3(nX, y, nZ + 0.5f),
                        loc2 = new Vector3(nX + 0.5f, y, nZ);
                MazeWall? wallX = (wargX != null) ? new MazeWall(ReplicateXWall(loc, new Quaternion()), wargX.Value.border) : (MazeWall?) null,
                          wallZ = (wargZ != null) ? new MazeWall(ReplicateZWall(loc2, Quaternion.Euler(0, 90, 0)), wargZ.Value.border) : (MazeWall?) null;
                MazeCell mazeWall = new MazeCell(wallX, wallZ);
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