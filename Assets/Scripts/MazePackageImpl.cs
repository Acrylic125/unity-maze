
using System;
using System.Collections.Generic;

namespace MazePackage {
    public class HuntKillMazeAlgorithm<T> 
            : MazeAlgorithm<T> where T : IMaze
    {

        private Stack<MazeCell> open;
        private LinkedList<MazeCell> closed;

        public HuntKillMazeAlgorithm(T maze): base(maze) {}

        public override void build() {
            open = new Stack<MazeCell>();
            closed = new LinkedList<MazeCell>();
            MazeMap map = maze.GetMazeMap();
        }

        private List<MazeCell> getAccessible() {
            List<MazeCell> accessible = new List<MazeCell>();
            
            return accessible;
        }

    }
}