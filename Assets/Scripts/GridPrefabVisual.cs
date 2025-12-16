using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEditor;

//Handles the prefaba of the grid
public class GridPrefabVisual : MonoBehaviour
{
    public AudioSource mineExplode;
    public bool played = false;
    public static GridPrefabVisual Instance { get; private set; }

    [SerializeField] private Transform pfGridPrefabVisualNode;
    [SerializeField] private GameObject tilePrefab;
    private List<Transform> visualNodeList;
    private Transform[,] visualNodeArray;
    private Grid<MapGridObject> grid;
    private bool updateVisual;
    //public GameObject hiddenTransform;

    private void Awake()
    {
        Instance = this;
        visualNodeList = new List<Transform>();
    }

    //Creates the grid of prefabs based on the grids positioning
    public void Setup(Grid<MapGridObject> grid)
    {
        this.grid = grid;
        visualNodeArray = new Transform[grid.GetWidth(), grid.GetHeight()];
        for(int i = 0; i < grid.GetWidth(); i++) {
            for (int j = 0; j < grid.GetHeight(); j++)
            {
                Vector3 worldPosition = grid.getWorldPosition(i, j);
                GameObject tileObject = Instantiate(tilePrefab, worldPosition, Quaternion.identity, transform);
                Debug.Log($"Tile {i},{j} position: {worldPosition}");
                visualNodeArray[i, j] = tileObject.transform;
                visualNodeList.Add(tileObject.transform);
                if (tileObject.GetComponent<Collider>() == null)
                {
                    tileObject.AddComponent<BoxCollider>();
                }
                TileComponent tileComponent = tileObject.GetComponent<TileComponent>();
                if (tileComponent == null)
                {
                    tileComponent = tileObject.AddComponent<TileComponent>();
                }
                tileComponent.Setup(i, j);
                tileObject.name = $"Tile ({i}, {j}";

                //Assign tile GameObject to its grid object
                MapGridObject gridObject = grid.GetGridObject(i, j);
                gridObject.SetTileObject(tileObject);
            }

        }

        HideNodeVisuals();
        UpdateVisual(grid);
        grid.OnGridObjectChange += Grid_OnGridObjectChanged;
    }

    private void Update()
    {
        {
            if (updateVisual)
            {
                updateVisual = false;
                UpdateVisual(grid);
            }
        }
    }
    
    //Update the prefab
    private void Grid_OnGridObjectChanged(object sender, Grid<MapGridObject>.OnGridObjectChangedEventArgs e)
    {
        updateVisual = true;
    }

    public void UpdateVisual(Grid<MapGridObject> grid)
    {
        HideNodeVisuals();

        for (int i = 0; i < grid.GetWidth(); i++)
        {
            for (int j = 0; j < grid.GetHeight(); j++)
            {
                MapGridObject gridObject = grid.GetGridObject(i, j);
                Transform visualNode = visualNodeArray[i, j];
                visualNode.gameObject.SetActive(true);
                SetupVisualNode(visualNode, gridObject);
            }
        }
    }

    //Sets the text of every tile to empty
    private void HideNodeVisuals()
    {
        foreach (Transform visualNodetransform in visualNodeList)
        {
            TextMeshPro text = visualNodetransform.Find("TileType").GetComponent<TextMeshPro>();
            text.text = "";
        }
    }

    private Transform CreateVisualNode(Vector3 position)
    {
        Transform visualNodeTransform = Instantiate(pfGridPrefabVisualNode, position, Quaternion.identity);
        return visualNodeTransform;
    }

    //Sets up the text of the prefab according to tile type
    private void SetupVisualNode(Transform visualNodeTransform, MapGridObject mapGridObject)
    {
        TextMeshPro indicatorText = visualNodeTransform.Find("TileType").GetComponent<TextMeshPro>();
        //indicatorText.SetText(mapGridObject.ToString());
        if (mapGridObject.IsRevealed())
        {
            switch (mapGridObject.GetGridType())
            {
                default:
                //Game end here
                case MapGridObject.Type.Mine:
                    indicatorText.SetText("Mine");

                    played = true;
                    break;
                case MapGridObject.Type.Empty:
                    indicatorText.SetText(" ");
                    break;
                case MapGridObject.Type.Mine_Num1:
                    indicatorText.SetText("1");
                    break;
                case MapGridObject.Type.Mine_Num2:
                    indicatorText.SetText("2");
                    break;
                case MapGridObject.Type.Mine_Num3:
                    indicatorText.SetText("3");
                    break;
                case MapGridObject.Type.Mine_Num4:
                    indicatorText.SetText("4");
                    break;
                case MapGridObject.Type.Mine_Num5:
                    indicatorText.SetText("5");
                    break;
                case MapGridObject.Type.Mine_Num6:
                    indicatorText.SetText("6");
                    break;
                case MapGridObject.Type.Mine_Num7:
                    indicatorText.SetText("7");
                    break;
                case MapGridObject.Type.Mine_Num8:
                    indicatorText.SetText("8");
                    break;
            }
        }
        else
        {
            indicatorText.text = "";
        }
    }
}

