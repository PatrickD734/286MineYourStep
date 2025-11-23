using UnityEngine;

public class MineSweeperGameHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    [SerializeField] private GridPrefabVisual gridPrefabVisual;
    private Map map;
    private bool isGameActive;
    public int flagCount = 40;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        map = new Map();
        gridPrefabVisual.Setup(map.GetGrid());
        isGameActive = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                Debug.Log("hit object: " + hitInfo.collider.name);
                GameObject clickedObject = hitInfo.collider.gameObject;
                TileComponent tile = clickedObject.GetComponent<TileComponent>();
                if (tile != null)
                {
                    HandleTileClick(tile.x, tile.y, clickedObject);
                    Debug.Log("Tile hit!");
                }
                else
                {
                    Debug.Log("Clicked object has no tile component");
                }
            }
            else
            {
                Debug.Log("Clicked nothing");
            }
        }

        // Right Click Detection
        else if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                GameObject clickedObject = hitInfo.collider.gameObject;
                TileComponent tile = clickedObject.GetComponent<TileComponent>();
                if (tile != null)
                {
                    HandleFlagClick(tile.x, tile.y, clickedObject);
                    Debug.Log("Flag handled!");
                }
            }
        }

        //Do an if statement for Input = 1(RightClick) Copy the code from above but instead of calling
        //HandleTileClick call HandleFlagClick

    }

    public static Vector3 GetMousePosition(Vector3 screenPosition, Camera worldCamera)
    {
        Ray ray = worldCamera.ScreenPointToRay(screenPosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        if (groundPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }

    private void HandleTileClick(int x, int y, GameObject tile)
    {
        MapGridObject mapGridObject = map.GetGrid().GetGridObject(x, y);
        if (mapGridObject == null) return;

        if (!tile.GetComponent<TileComponent>().isFlagged)
        {
            MapGridObject.Type gridType = map.RevealGridPosition(mapGridObject);
        }
    }

    private void HandleFlagClick(int x, int y, GameObject tile)
    {
        MapGridObject mapGridObject = map.GetGrid().GetGridObject(x, y);
        TileComponent tileComponent = tile.GetComponent<TileComponent>();
        if (!tileComponent.isFlagged && flagCount > 0 && !mapGridObject.IsRevealed())
        {
            //Gets the flag prefab from the GameObject tile to set active true
            Transform flagTransform = tile.transform.Find("Flag");
            if (flagTransform != null)
            {
                flagTransform.gameObject.SetActive(true);
            }
            //set tile component is flagged to true
            tileComponent.isFlagged = true;
            flagCount--;
        }
        else if(flagCount < 40)
        {
            //Gets the flag prefab from the GameObject tile to set active false
            Transform flagTransform = tile.transform.Find("Flag");
            if (flagTransform != null)
            {
                flagTransform.gameObject.SetActive(false);
            }
            //set tile component is flagged to false
            tileComponent.isFlagged = false;
            flagCount++;
        }
    }

}
