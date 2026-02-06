//
// This script executes the change of camera while being triggered via collisionwith the player.
//
// To make this script work, create and box and make it mesh invisible, after that assign the player object as "Player" in player object inspector tag menu. Enable collision for both trigger and player objects(also make sure you enable rigidbody for the player).
// Drop script at created trigger object inspector and assign two cameras we want to change.
// Make sure that first camera that we want to be old when having transition is on and the new camera that we want to turn on after is deactivated(checkbox ticked off). And designate the player object as Player.
//

using UnityEngine;

public class CamChange : MonoBehaviour
{
    public GameObject camOld;
    public GameObject camNew;
    public Walking playerWalking;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // if-statement when object tagged as Player, works better rather than just comparing for a name
        {
            camOld.SetActive(false);
            camNew.SetActive(true);

            playerWalking.SetCamera(camNew.transform); // will make the player movement relative to the new camera so it won't be disoriented after transition
        }
    }
}
