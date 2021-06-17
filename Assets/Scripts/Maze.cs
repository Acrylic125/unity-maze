using UnityEngine;
using MazeAlgo;

public class Maze : MonoBehaviour, IMaze
{

    public Bounds bb = new Bounds(new Vector3(0, 0, 0), new Vector3(40, 1, 40));
    public int xCells = 20, zCells = 20;
    public GameObject wallCloneable;
    private MazeMap mazeMap;

    public Bounds GetBorder() {
        return bb;
    }
        
    public int GetXCells() {
        return xCells;
    }
        
    public int GetZCells() {
        return zCells;
    }

    public float GetXWallFactor() {
        return bb.size.x / xCells;
    }
    public float GetZWallFactor() {
        return bb.size.z / zCells;
    }

    public MazeMap GetMazeMap() {
        return mazeMap;
    }

    public GameObject WallCloneable() {
        return wallCloneable;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.mazeMap = new MazeMap(xCells, zCells);
        Debug.Log(GetXWallFactor() + " " + GetZWallFactor());
    }

    // Update is called once per frame
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(bb.center, bb.size);
    }
        
}
