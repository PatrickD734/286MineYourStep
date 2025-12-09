using UnityEngine;

//Represents one tile posititon and contains all the tile types
public class MapGridObject
{
    private GameObject tileObject;

    public void SetTileObject(GameObject obj)
    {
        tileObject = obj;
    }

    public GameObject GetTileObject()
    {
        return tileObject;
    }
    public enum Type
    {
        Empty,
        Mine,
        Mine_Num1,
        Mine_Num2,
        Mine_Num3,
        Mine_Num4,
        Mine_Num5,
        Mine_Num6,
        Mine_Num7,
        Mine_Num8,
    }
    private Grid<MapGridObject> grid;
    private int x;
    private int y;
    private Type type;
    private bool isRevealed;
    public MapGridObject(Grid<MapGridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        type = Type.Empty;
        isRevealed = false;
    }

    public Type GetGridType()
    {
        return type;
    }

    public int GetX() => x;
    public int GetY() => y;
    public void SetGridType(Type type)
    {
        this.type = type;
        grid.TriggerGridObjectChanged(x, y);
        //Debug.Log($"SetGridType called at ({x},{y}) -> {type}");
    }

    public void Reveal()
    {
        isRevealed = true;
        grid.TriggerGridObjectChanged(x, y);
    }

    public bool IsRevealed()
    {
        return isRevealed;
    }
    public override string ToString()
    {
        return type.ToString();
    }

}
