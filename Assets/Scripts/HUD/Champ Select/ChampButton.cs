using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChampButton : MonoBehaviour {

    public NetworkManager network;
    public GameManager game;
    public Champions champ;
    private Button button;

	// Use this for initialization
	void Start () {
	}

    public void onClicked()
    {
        game.selectChampion(champ, network.playerID);
    }
	
}
