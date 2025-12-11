using UnityEngine;
using System.Collections.Generic;

public class Map
{
    private bool firstTileClick = false;
    private Grid<MapGridObject> grid;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Map()
    {
        //Creates a grid using the Grid code File
        grid = new Grid<MapGridObject>(16, 16, 5f, Vector3.zero, (Grid<MapGridObject> g, int x, int y) => new MapGridObject(g, x, y));
    }
    
    //Generates a map of 40 mines but avoids placing mines in the area of the a person's first click
    private void generateMap(int firstX, int firstY)
    {
        int minesPlaced = 0;
        int mineAmount = 40;
        //Get neighbours of first clicked tile
        List<MapGridObject> firstNeighbourList = GetNeighbourList(firstX, firstY);
        while (minesPlaced < mineAmount)
        {
            //Gets a random tile from grid's width and height range
            int x = UnityEngine.Random.Range(0, grid.GetWidth());
            int y = UnityEngine.Random.Range(0, grid.GetHeight());

            //Creates a mapGridObject based on that specific tile
            MapGridObject mapGridObject = grid.GetGridObject(x, y);
            //Compares the tile to first tile
            if (mapGridObject.GetGridType() != MapGridObject.Type.Mine && !(x == firstX && y == firstY))
            {
                //Compares tiles to first tile's neighbours
                if(!checkNeighbourList(firstNeighbourList, x, y))
                {
                    //Changes tile to a Mine if all conditions are passed
                    mapGridObject.SetGridType(MapGridObject.Type.Mine);
                    minesPlaced++;
                }        
            }
        }

        //Sets the rest of the non-mine tiles to respective numbered tiles
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                MapGridObject mapGridObject = grid.GetGridObject(x, y);
                if (mapGridObject.GetGridType() == MapGridObject.Type.Empty)
                {
                    //Makes a list of a tile's neighbours
                    List<MapGridObject> neighbourList = GetNeighbourList(x, y);
                    int mineCount = 0;
                    //Checks every neighbour for mines and adds it to minecount
                    foreach (MapGridObject neighbour in neighbourList)
                    {
                        if (neighbour.GetGridType() == MapGridObject.Type.Mine)
                        {
                            mineCount++;
                        }
                    }

                    //Check mineCount and set tile to respective tile type
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
    //GetNeighbourList functions to create a list of tiles next to the arguement tile
    private List<MapGridObject> GetNeighbourList(MapGridObject mapGridObject)
    {
        return GetNeighbourList(mapGridObject.GetX(), mapGridObject.GetY());
    }
    private List<MapGridObject> GetNeighbourList(int x, int y)
    {
        List<MapGridObject> neighbourList = new List<MapGridObject>();
        //Checks all directions
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

    //checkNeighbourList function to see if a tile is in a neighbour list 
    private bool checkNeighbourList(List<MapGridObject> neighbourlist, int x, int y)
    {
        foreach (MapGridObject neighbour in neighbourlist)
        {
            if (neighbour.GetX() == x && neighbour.GetY() == y)
            {
                return true;
            }
        }
        return false;
    }

    public Grid<MapGridObject> GetGrid()
    {
        return grid;
    }
    
    //Reveals position of a tile
    
    public MapGridObject.Type RevealGridPosition(Vector3 worldPosition)
    {
        MapGridObject mapGridObject = grid.GetGridObject(worldPosition);
        return RevealGridPosition(mapGridObject);
    }
    
    //Reveal Grid Positon, add Lose Condition for hitting a Mine!
    public MapGridObject.Type RevealGridPosition(MapGridObject mapGridObject)
    {
        //Generates the map after the first click
        //Checks it the click is the first click
        if (!firstTileClick)
        {
            generateMap(mapGridObject.GetX(), mapGridObject.GetY());
            firstTileClick = true;
        }
        mapGridObject.Reveal();
        GameObject tileObject = mapGridObject.GetTileObject();
        if (tileObject == null) return mapGridObject.GetGridType();
        Transform dirtPile = tileObject.transform.Find("Dirt");
        dirtPile.gameObject.SetActive(false);
        //Checks if neighbour tile is empty and reveals those tiles too
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
                        GameObject negTileObject = neighbour.GetTileObject();
                        if (negTileObject == null) continue;
                        Transform neighbourDirtPile = negTileObject.transform.Find("Dirt");
                        neighbourDirtPile.gameObject.SetActive(false);
                    }
                }

            }
        }
        return mapGridObject.GetGridType();
    }

}
