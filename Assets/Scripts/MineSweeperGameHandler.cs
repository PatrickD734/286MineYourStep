using UnityEngine;

public class MineSweeperGameHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private GridPrefabVisual gridPrefabVisual;
    private Map map;
    private bool isGameActive;
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
            if(Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                Debug.Log("hit object: " + hitInfo.collider.name);
                GameObject clickedObject = hitInfo.collider.gameObject;
                TileComponent tile = clickedObject.GetComponent<TileComponent>();
                if (tile != null)
                {
                    HandleTileClick(tile.x, tile.y);
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

    private void HandleTileClick(int x, int y)
    {
        MapGridObject mapGridObject = map.GetGrid().GetGridObject(x, y);
        if (mapGridObject == null) return;

        MapGridObject.Type gridType = map.RevealGridPosition(mapGridObject);

        if(gridType == MapGridObject.Type.Mine)
        {
            Debug.Log("Mine found!");
        }
    }
}
