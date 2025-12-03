using UnityEngine;

public class MiniMapFollows : MonoBehaviour
{
    [SerializeField] private Transform player; // player object
    [SerializeField] private float height = 30f; // Height above player object

    void LateUpdate()
    {
        if (player == null) {
            return; 
        } //basically saying if no player object exists, have the output be nothing
        else
        {
            Vector3 newPosition = player.position;
            newPosition.y += height;
            transform.position = newPosition; //All 3 lines track player object position

            transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f); //Locks position to top down and has the map spin around to match where player is going 
        }

    }
}
