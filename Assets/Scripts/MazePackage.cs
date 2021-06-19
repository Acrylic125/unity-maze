
using UnityEngine;

namespace MazePackage
{

    public struct MazeWall {

        public GameObject wall;
        public bool blocked, border;
        
        public MazeWall(GameObject wall, MazeMap.WallArgs args) {
            this.wall = wall;
            this.blocked = args.blocked;
            this.border = args.border;
            if (border) {
                foreach(var renderer in wall.GetComponentsInChildren<Renderer>(true)) 
                    renderer.material.color = new Color(44, 44, 44);
            }
        }

        public void Destroy() {
            if (wall != null)
                Object.Destroy(wall);
        }

    }

    public struct MazeCell {
        public readonly int x, z;
        public MazeWall? posXWall, posZWall;

        public MazeCell(int x, int z, MazeWall? posXWall, MazeWall? posZWall)
        {
            this.x = x;
            this.z = z;
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
            AddCell(0, 0, WallArgs.Border, WallArgs.Border);
            AddCell(xCells, 0, WallArgs.Border, null);
            AddCell(0, zCells, null, WallArgs.Border);
            for (int x = 1; x < xCells; x++) {
                AddCell(x, 0, WallArgs.Normal, WallArgs.Border);
                AddCell(x, zCells, null, WallArgs.Border);
            }
            for (int z = 1; z < zCells; z++) {
                AddCell(0, z, WallArgs.Border, WallArgs.Normal);
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
            
            public static readonly WallArgs Border = new WallArgs(true, true);
            public static readonly WallArgs Normal = new WallArgs(false, false);
            
            public readonly bool blocked, border;

            WallArgs(bool blocked, bool border) {
                this.blocked = blocked;
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
                MazeWall? wallX = (wargX != null) ? new MazeWall(ReplicateXWall(loc, new Quaternion()), wargX.Value) : (MazeWall?) null,
                          wallZ = (wargZ != null) ? new MazeWall(ReplicateZWall(loc2, Quaternion.Euler(0, 90, 0)), wargZ.Value) : (MazeWall?) null;
                MazeCell mazeWall = new MazeCell(x, z, wallX, wallZ);
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