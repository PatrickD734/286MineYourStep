using UnityEngine;

//Handles the game of Minesweeper
public class MineSweeperGameHandler : MonoBehaviour
{
    public AudioSource digSound;
    public AudioSource background;
    public AudioSource mineExplode;
    [SerializeField] private GridPrefabVisual gridPrefabVisual;
    [SerializeField] private LoseWin loseWinHandler;

    private Map map;
    private bool isGameActive;
    public int flagCount = 40;
    

    void Start()
    {
        isGameActive = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        map = new Map();
        gridPrefabVisual.Setup(map.GetGrid());
        isGameActive = true;
        background.Play();
    }

    private void Update()
    {
        // KEYBOARD SHORTCUTS 
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("TEST: Force Game Over (Lose) with P key");
            ForceGameOver(false); // false = lose
            return;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("TEST: Force Victory (Win) with O key");
            ForceGameOver(true); // true = win
            return;
        }

        // Restart shortcut - R key OR Right Click
        if (Input.GetKeyDown(KeyCode.R) || (Input.GetMouseButtonDown(1) && !isGameActive))
        {
            Time.timeScale = 1;
            Debug.Log("Restarting game with R key or Right Click");
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            return;
        }

        if (!isGameActive || map.IsGameEnded()) return;

        if (Input.GetMouseButtonDown(0))
        {
            HandleLeftClick();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            HandleRightClick();
        }
    }

     
    private void ForceGameOver(bool didWin)
    {
        if (isGameActive) // Only trigger if game is still active
        {
            isGameActive = false;

            // Force the map to think game ended using the ForceEndGame method
            map.ForceEndGame(didWin);

            if (loseWinHandler != null)
            {
                if (didWin)
                {
                    loseWinHandler.ShowWinScreen();
                }
                else
                {
                    loseWinHandler.ShowLoseScreen();
                }
            }
            else
            {
                ShowFallbackUI(didWin); // Pass the win/lose state
            }
        }
    }

    public static MineSweeperGameHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void PlayerHitMine()
    {

        if (!isGameActive) return;
        mineExplode.Play();
        Debug.Log("Player touched a mine!");
        ForceGameOver(false); // lose
    }
    private void HandleLeftClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            GameObject clickedObject = hitInfo.collider.gameObject;
            TileComponent tile = clickedObject.GetComponent<TileComponent>();
            if (tile != null)
            {
                HandleTileClick(tile.x, tile.y, clickedObject);
                digSound.Play();
            }
        }
    }

    private void HandleRightClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            GameObject clickedObject = hitInfo.collider.gameObject;
            TileComponent tile = clickedObject.GetComponent<TileComponent>();
            if (tile != null)
            {
                HandleFlagClick(tile.x, tile.y, clickedObject);
            }
        }
    }

    private void HandleTileClick(int x, int y, GameObject tile)
    {
        MapGridObject mapGridObject = map.GetGrid().GetGridObject(x, y);
        if (mapGridObject == null) return;

        if (!tile.GetComponent<TileComponent>().isFlagged)
        {
            MapGridObject.Type gridType = map.RevealGridPosition(mapGridObject);

            if (map.IsGameEnded())
            {
                Debug.Log("GAME ENDED: " + map.GetGameState());
                HandleGameEnd();
            }
        }
    }

    private void HandleGameEnd()
    {
        isGameActive = false;

        if (loseWinHandler != null)
        {
            if (map.DidPlayerWin())
            {
                loseWinHandler.ShowWinScreen();
            }
            else
            {
                mineExplode.Play();
                loseWinHandler.ShowLoseScreen();
            }
        }
        else
        {
           
            ShowFallbackUI(map.DidPlayerWin()); // Pass the win/lose state
        }
    }

   
    private void ShowFallbackUI(bool didWin)
    {
        Time.timeScale = 0;
        Debug.Log("Creating fallback UI");

        GameObject canvasObj = new GameObject("GameOverCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        GameObject panel = new GameObject("Panel");
        panel.transform.SetParent(canvas.transform);
        UnityEngine.UI.Image panelImage = panel.AddComponent<UnityEngine.UI.Image>();
        panelImage.color = new Color(0, 0, 0, 0.8f);

        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        GameObject textObj = new GameObject("GameOverText");
        textObj.transform.SetParent(panel.transform);
        UnityEngine.UI.Text text = textObj.AddComponent<UnityEngine.UI.Text>();
        text.text = didWin ? "VICTORY!" : "GAME OVER!"; // Use parameter
        text.fontSize = 48;
        text.color = didWin ? Color.green : Color.red; // Use parameter
        text.alignment = TextAnchor.MiddleCenter;
        text.font = UnityEngine.Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        RectTransform textRect = text.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0.2f, 0.4f);
        textRect.anchorMax = new Vector2(0.8f, 0.6f);
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        // Create Restart Button
        GameObject btnObj = new GameObject("RestartButton");
        btnObj.transform.SetParent(panel.transform);

        // REQUIRED components
        RectTransform btnRect = btnObj.AddComponent<RectTransform>();
        UnityEngine.UI.Image btnImage = btnObj.AddComponent<UnityEngine.UI.Image>();
        UnityEngine.UI.Button btn = btnObj.AddComponent<UnityEngine.UI.Button>();

        btnImage.color = new Color(0.2f, 0.2f, 0.2f, 1f);

        // Position
        btnRect.anchorMin = new Vector2(0.3f, 0.2f);
        btnRect.anchorMax = new Vector2(0.7f, 0.3f);
        btnRect.offsetMin = Vector2.zero;
        btnRect.offsetMax = Vector2.zero;

        // Create Text child
        GameObject btnTextObj = new GameObject("Text");
        btnTextObj.transform.SetParent(btnObj.transform);

        UnityEngine.UI.Text btnText = btnTextObj.AddComponent<UnityEngine.UI.Text>();
        btnText.text = "RESTART (R Key)";
        btnText.fontSize = 24;
        btnText.color = Color.white;
        btnText.alignment = TextAnchor.MiddleCenter;
        btnText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        // Text Rect
        RectTransform textButton = btnText.GetComponent<RectTransform>();
        textButton.anchorMin = Vector2.zero;
        textButton.anchorMax = Vector2.one;
        textButton.offsetMin = Vector2.zero;
        textButton.offsetMax = Vector2.zero;

        // Button action
        btn.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        });
    }

    private void HandleFlagClick(int x, int y, GameObject tile)
    {
        MapGridObject mapGridObject = map.GetGrid().GetGridObject(x, y);
        TileComponent tileComponent = tile.GetComponent<TileComponent>();

        if (!tileComponent.isFlagged && flagCount > 0 && !mapGridObject.IsRevealed())
        {
            Transform flagTransform = tile.transform.Find("Flag");
            if (flagTransform != null) flagTransform.gameObject.SetActive(true);
            tileComponent.isFlagged = true;
            flagCount--;
        }
        else if (tileComponent.isFlagged && flagCount < 40)
        {
            Transform flagTransform = tile.transform.Find("Flag");
            if (flagTransform != null) flagTransform.gameObject.SetActive(false);
            tileComponent.isFlagged = false;
            flagCount++;
        }
    }
}