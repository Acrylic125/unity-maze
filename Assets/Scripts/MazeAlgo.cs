
using UnityEngine;

namespace MazeAlgo
{

    public enum MazeWallState {
        PosX, PosZ
    }

    public struct MazeWall {
        public GameObject gameObject;
        public bool destroyable;
        public MazeWallState wallState;

        public MazeWall(GameObject gameObject, bool destroyable, MazeWallState wallState)
        {
            this.gameObject = gameObject;
            this.destroyable = destroyable;
            this.wallState = wallState;
        }
    }

    public class MazeMap {

        public MazeWall[,] walls;
        public GameObject posXWall, posZWall;

        public MazeMap(GameObject wallObject, IMaze maze) {
            setWallType(wallObject);
            this.walls = new MazeWall[maze.GetXCells(), maze.GetZCells()];
        }

        public void setWallType(GameObject wallObject) {
            
        }

        public void AddWall(int x, int z, MazeWall mazeWall) {
            walls.SetValue(mazeWall, x, z);
        }

        public void RemoveWall(int x, int z) {
            walls.SetValue(null, x, z);
        }
       
    }

    public interface IMaze {
        Bounds GetBorder();
        
        int GetXCells();
        
        int GetZCells();

        float GetXWallFactor();

        float GetZWallFactor();
        
        MazeMap GetMazeMap();
    }

    public abstract class MazeAlgorithm<T>
                    where T : IMaze {
        
        public T maze;
        
        public MazeAlgorithm(T maze) {
            this.maze = maze;
        }

        public abstract void build();

    }

    public class HuntKillMazeAlgorithm<T> 
            : MazeAlgorithm<T> where T : IMaze
    {

        public HuntKillMazeAlgorithm(T maze): base(maze) {}

        public override void build() {

        }

    }



}