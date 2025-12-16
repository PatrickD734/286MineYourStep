using System.Runtime.CompilerServices;
using UnityEngine;

public class TileComponent : MonoBehaviour
{
    public Renderer rend;
    private Color oldColor;
    private Color hoverColor = Color.white;
    public bool isFlagged;
    public GameObject flagInstance;

    public int x;
    public int y;

    private Collider mineDetector;

    private void Awake()
    {
        // Find the MineDetector collider on this tile
        Transform detector = transform.Find("MineDetector");
        if (detector != null)
            mineDetector = detector.GetComponent<Collider>();
    }

    // Call this whenever the flag state changes
    public void UpdateFlaggedState(bool flagged)
    {
        isFlagged = flagged;

        if (mineDetector != null)
        {
            mineDetector.enabled = !flagged; // disable collider if flagged
        }
    }
    public void Setup(int x, int y)
    {
        //rend = GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            oldColor = rend.material.color;
        }
        this.x = x;
        this.y = y;
    }

    //Changes tile color of mouse hover
    void OnMouseEnter()
    {
        Debug.Log("Mouse Entered");
        if (rend != null)
        {
            rend.material.color = hoverColor;
        }
        else
        {
            Debug.Log("No renderered!");
        }
    }

    //returns tile to old color on mouse exit
    void OnMouseExit()
    {
        Debug.Log("Mouse Exited");
        if (rend != null)
        {
            rend.material.color = oldColor;
        }
    }
}
