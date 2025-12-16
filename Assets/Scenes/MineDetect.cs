using UnityEngine;

public class PlayerMineDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mine") )
        {
            MineSweeperGameHandler.Instance.PlayerHitMine();
        }
    }
}
