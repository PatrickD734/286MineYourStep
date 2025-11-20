using UnityEngine;
using System.Collections.Generic;

public class Map
{
    private Grid<MapGridObject> grid;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Map()
    {
        grid = new Grid<MapGridObject>(16, 16, 5f, Vector3.zero, (Grid<MapGridObject> g, int x, int y) => new MapGridObject(g, x, y));
        int minesPlaced = 0;
        int mineAmount = 40;

        while(minesPlaced < mineAmount)
        {
            int x = UnityEngine.Random.Range(0, grid.GetWidth());
            int y = UnityEngine.Random.Range(0, grid.GetHeight());

            MapGridObject mapGridObject = grid.GetGridObject(x, y);
            if(mapGridObject.GetGridType() != MapGridObject.Type.Mine)
            {
                mapGridObject.SetGridType(MapGridObject.Type.Mine);
                minesPlaced++;
                //Debug.Log("Mine Placed");
            }
        }

        for(int x = 0; x < grid.GetWidth(); x++)
        {
            for(int y = 0; y < grid.GetHeight(); y++)
            {
                MapGridObject mapGridObject = grid.GetGridObject(x, y);
                if(mapGridObject.GetGridType() == MapGridObject.Type.Empty)
                {
                    List<MapGridObject> neighbourList = GetNeighbourList(x, y);
                    int mineCount = 0;
                    foreach(MapGridObject neighbour in neighbourList)
                    {
                        if(neighbour.GetGridType() == MapGridObject.Type.Mine)
                        {
                            mineCount++;
                        }
                    }
                    
                    switch (mineCount)
                    {
                        case 1: mapGridObject.SetGridType(MapGridObject.Type.Mine_Num1); break;
                        case 2: mapGridObject.SetGridType(MapGridObject.Type.Mine_Num2); break;
                        case 3: mapGridObject.SetGridType(MapGridObject.Type.Mine_Num3); break;
                        case 4: mapGridObject.SetGridType(MapGridObject.Type.Mine_Num4); break;
                        case 5: mapGridObject.SetGridType(MapGridObject.Type.Mine_Num5); break;
                        case 6: mapGridObject.SetGridType(MapGridObject.Type.Mine_Num6); break;
                        case 7: mapGridObject.SetGridType(MapGridObject.Type.Mine_Num7); break;
                        case 8: mapGridObject.SetGridType(MapGridObject.Type.Mine_Num8); break;
                    }
                }
            }
        }
    }
    
    private List<MapGridObject> GetNeighbourList(MapGridObject mapGridObject)
    {
        return GetNeighbourList(mapGridObject.GetX(), mapGridObject.GetY());
    }
    private List<MapGridObject> GetNeighbourList(int x, int y)
    {
        List<MapGridObject> neighbourList = new List<MapGridObject>();
        
        if (x - 1 >= 0)
        {
            //Left
            neighbourList.Add(grid.GetGridObject(x - 1, y));
            //Left-down
            if(y - 1 >= 0) neighbourList.Add(grid.GetGridObject(x - 1, y - 1));
            //Left-up
            if(y + 1 < grid.GetHeight()) neighbourList.Add(grid.GetGridObject(x - 1, y + 1));
        }
        if (x + 1 < grid.GetWidth())
        {
            //Right
            neighbourList.Add(grid.GetGridObject(x + 1, y));
            //Right-down
            if (y - 1 >= 0) neighbourList.Add(grid.GetGridObject(x + 1, y - 1));
            //Right-up
            if (y + 1 < grid.GetHeight()) neighbourList.Add(grid.GetGridObject(x + 1, y + 1));
        }
        //Up
        if (y + 1 < grid.GetHeight()) neighbourList.Add(grid.GetGridObject(x, y + 1));
        //Down
        if (y - 1 >= 0) neighbourList.Add(grid.GetGridObject(x, y - 1));

        return neighbourList;

    }

    public Grid<MapGridObject> GetGrid()
    {
        return grid;
    }
    
    public MapGridObject.Type RevealGridPosition(Vector3 worldPosition)
    {
        MapGridObject mapGridObject = grid.GetGridObject(worldPosition);
        return RevealGridPosition(mapGridObject);
    }
    public MapGridObject.Type RevealGridPosition(MapGridObject mapGridObject)
    {
        mapGridObject.Reveal();
        if (mapGridObject.GetGridType() == MapGridObject.Type.Empty)
        {
            List<MapGridObject> checkNeighbourList = new List<MapGridObject>();
            List<MapGridObject> neighbourList = new List<MapGridObject>();
            neighbourList.Add(mapGridObject);

            while (neighbourList.Count > 0)
            {
                MapGridObject checkMapGridObject = neighbourList[0];
                neighbourList.RemoveAt(0);
                checkNeighbourList.Add(checkMapGridObject);
                foreach (MapGridObject neighbour in GetNeighbourList(checkMapGridObject))
                {
                    if (neighbour.GetGridType() != MapGridObject.Type.Mine)
                    {
                        neighbour.Reveal();
                        if (neighbour.GetGridType() == MapGridObject.Type.Empty)
                        {
                            // If empty, check add it to queue
                            if (!checkNeighbourList.Contains(neighbour))
                            {
                                neighbourList.Add(neighbour);
                            }
                        }
                    }
                }

            }
        }
        return mapGridObject.GetGridType();
    }

}
