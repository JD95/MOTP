using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UI = UnityEngine.UI;

public class ReadyButton : MonoBehaviour {

    public GameObject gameManager;
    private GameManager game;
    private UI.Button button;

	// Use this for initialization
	void Start () {
        game = gameManager.GetComponent<GameManager>();

        button = GetComponent<UI.Button>();

        
	}
	
    public void onClicked()
    {
        game.playerIsReady();
        GetComponent<Button>().interactable = false;
    }
}
