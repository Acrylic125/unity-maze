using UnityEngine;
using MazePackage;

public class Maze : MonoBehaviour, IMaze
{

    public Vector3 start = new Vector3(20, 0, 20);
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

    public float GetCellXSize() {
        return bb.size.x / xCells;
    }

    public float GetCellZSize() {
        return bb.size.z / zCells;
    }

    public MazeMap GetMazeMap() {
        return mazeMap;
    }

    public Vector3 GetStarting() {
        return start;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.mazeMap = new MazeMap(wallCloneable, this);
        Debug.Log(GetXWallFactor() + " " + GetZWallFactor());
    }

    // Update is called once per frame
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(bb.center, bb.size);
    }
        
}
