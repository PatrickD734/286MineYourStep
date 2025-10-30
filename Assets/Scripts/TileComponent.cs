using UnityEngine;

public class TileComponent : MonoBehaviour
{
    public int x;
    public int y;

    public void Setup(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
