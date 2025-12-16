using UnityEngine;
using System.Collections.Generic;

public class Map
{
    private bool firstTileClick = false;
    private Grid<MapGridObject> grid;
    private int totalMines = 40;
    private int revealedTiles = 0;
    private int totalTiles;
    private bool gameEnded = false;

    public Map()
    {
        grid = new Grid<MapGridObject>(16, 16, 5f, Vector3.zero,
            (Grid<MapGridObject> g, int x, int y) => new MapGridObject(g, x, y));
        totalTiles = grid.GetWidth() * grid.GetHeight();
    }

    private void generateMap(int firstX, int firstY)
    {
        int minesPlaced = 0;
        int mineAmount = 40;
        List<MapGridObject> firstNeighbourList = GetNeighbourList(firstX, firstY);

        while (minesPlaced < mineAmount)
        {
            int x = UnityEngine.Random.Range(0, grid.GetWidth());
            int y = UnityEngine.Random.Range(0, grid.GetHeight());

            MapGridObject mapGridObject = grid.GetGridObject(x, y);
            if (mapGridObject.GetGridType() != MapGridObject.Type.Mine &&
                !(x == firstX && y == firstY))
            {
                if (!checkNeighbourList(firstNeighbourList, x, y))
                {
                    mapGridObject.SetGridType(MapGridObject.Type.Mine);
                    minesPlaced++;
                }
            }
        }

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                MapGridObject mapGridObject = grid.GetGridObject(x, y);
                if (mapGridObject.GetGridType() == MapGridObject.Type.Empty)
                {
                    List<MapGridObject> neighbourList = GetNeighbourList(x, y);
                    int mineCount = 0;

                    foreach (MapGridObject neighbour in neighbourList)
                    {
                        if (neighbour.GetGridType() == MapGridObject.Type.Mine)
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

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                MapGridObject tile = grid.GetGridObject(x, y);
                if (tile.GetGridType() != MapGridObject.Type.Mine)
                {
                    GameObject mineTileObject = tile.GetTileObject();
                    if (mineTileObject != null)
                    {
                        Transform mineDetect = mineTileObject.transform.Find("MineDetector");
                        if (mineDetect != null)
                        {
                            GameObject.Destroy(mineDetect.gameObject);
                            // or DestroyImmediate(mineDetect.gameObject); // only if needed in editor
                        }
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
            neighbourList.Add(grid.GetGridObject(x - 1, y));
            if (y - 1 >= 0) neighbourList.Add(grid.GetGridObject(x - 1, y - 1));
            if (y + 1 < grid.GetHeight()) neighbourList.Add(grid.GetGridObject(x - 1, y + 1));
        }
        if (x + 1 < grid.GetWidth())
        {
            neighbourList.Add(grid.GetGridObject(x + 1, y));
            if (y - 1 >= 0) neighbourList.Add(grid.GetGridObject(x + 1, y - 1));
            if (y + 1 < grid.GetHeight()) neighbourList.Add(grid.GetGridObject(x + 1, y + 1));
        }

        if (y + 1 < grid.GetHeight()) neighbourList.Add(grid.GetGridObject(x, y + 1));
        if (y - 1 >= 0) neighbourList.Add(grid.GetGridObject(x, y - 1));

        return neighbourList;
    }

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

    public MapGridObject.Type RevealGridPosition(Vector3 worldPosition)
    {
        MapGridObject mapGridObject = grid.GetGridObject(worldPosition);
        return RevealGridPosition(mapGridObject);
    }

    public MapGridObject.Type RevealGridPosition(MapGridObject mapGridObject)
    {
        if (gameEnded) return mapGridObject.GetGridType();

        if (!firstTileClick)
        {
            generateMap(mapGridObject.GetX(), mapGridObject.GetY());
            firstTileClick = true;
        }

        if (mapGridObject.GetGridType() == MapGridObject.Type.Mine)
        {
            gameEnded = true;

            mapGridObject.Reveal();
            GameObject mineTileObject = mapGridObject.GetTileObject();
            if (mineTileObject != null)
            {
                Transform mineDirtPile = mineTileObject.transform.Find("Dirt");
                if (mineDirtPile != null)
                {
                    mineDirtPile.gameObject.SetActive(false);
                }
            }

            return MapGridObject.Type.Mine;
        }

        if (!mapGridObject.IsRevealed())
        {
            revealedTiles++;
        }

        mapGridObject.Reveal();
        GameObject tileObject = mapGridObject.GetTileObject();
        if (tileObject == null) return mapGridObject.GetGridType();
        Transform dirtPile = tileObject.transform.Find("Dirt");
        dirtPile.gameObject.SetActive(false);

        CheckWinCondition();

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
                        if (!neighbour.IsRevealed())
                        {
                            revealedTiles++;
                        }

                        neighbour.Reveal();
                        CheckWinCondition();

                        if (neighbour.GetGridType() == MapGridObject.Type.Empty)
                        {
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

    private void CheckWinCondition()
    {
        if (gameEnded) return;
        int nonMineTiles = totalTiles - totalMines;
        if (revealedTiles >= nonMineTiles)
        {
            gameEnded = true;
        }
    }

    public bool IsGameEnded()
    {
        return gameEnded;
    }

    public bool DidPlayerWin()
    {
        if (!gameEnded) return false;
        int nonMineTiles = totalTiles - totalMines;
        return revealedTiles >= nonMineTiles;
    }

    public string GetGameState()
    {
        if (!gameEnded) return "Playing";
        return DidPlayerWin() ? "Won" : "Lost";
    }

    // ADD THIS METHOD FOR KEYBOARD SHORTCUTS (P and O keys)
    public void ForceEndGame(bool didWin)
    {
        gameEnded = true;

        if (didWin)
        {
            // For win: set revealed tiles to match win condition
            revealedTiles = totalTiles - totalMines;
        }
        // For lose: gameEnded is already true, no need to change revealedTiles
        // The mines will remain hidden
    }
}