using UnityEngine;

/* 
	This handles what attributes of the object are synced across the network.
	Mostly these would be things related to models and animations and things like
	health and money would be stored in some kind of central location like the game
	manager.
 */

public class NetworkCharacter : Photon.MonoBehaviour
{
    private Vector3 correctPlayerPos = Vector3.zero; // We lerp towards this
    private Quaternion correctPlayerRot = Quaternion.identity; // We lerp towards this
    
	// Update is called once per frame
    void Update()
    {
        if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

			// Send animation state
			Character myChar = GetComponent<Character>();
			stream.SendNext((int)myChar.currentAnimation);
        }
        else
        {
            // Sync player rotatation and position
            this.correctPlayerPos = (Vector3)stream.ReceiveNext();
            this.correctPlayerRot = (Quaternion)stream.ReceiveNext();

			// Sync Character animations
			Character myChar = GetComponent<Character>();
			myChar.currentAnimation = (Animations)stream.ReceiveNext();
        }
    }
}
