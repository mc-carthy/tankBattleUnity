using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

using System.Collections;

public class GameManager : NetworkBehaviour {

	[SyncVarAttribute]
	private int playerCount = 0;

	private static GameManager instance;
	public static GameManager Instance {
		get {
			if (instance == null) {
				instance = GameObject.FindObjectOfType<GameManager>();

				if (instance == null) {
					instance = new GameObject().AddComponent<GameManager>();
				}
			}
			return instance;
		}
	}

	[SerializeField]
	private Text messageText;	
	
	private Color[] playerColors = { Color.red, Color.blue, Color.green, Color.yellow };
	private int minPlayers = 1;
	private int maxPlayers = 4;

	private void Awake () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}

	private void Start () {
		StartCoroutine(GameLoopRoutine());
	}

	public void AddPlayer (PlayerSetup pSetup) {
		if (playerCount < maxPlayers) {
			pSetup.PlayerColor = playerColors[playerCount];
			playerCount++;
			pSetup.PlayerNum = playerCount;
		}
	}

	private IEnumerator GameLoopRoutine() {
		yield return StartCoroutine(EnterLobby());
		yield return StartCoroutine(PlayGame());
		yield return StartCoroutine(EndGame());
	}

	private IEnumerator EnterLobby () {

		if (messageText != null) {
			messageText.gameObject.SetActive(true);
			messageText.text = "Waiting for other players...";
		}
		
		while (playerCount < minPlayers) {
			DisablePlayers();
			yield return null;
		}
	}

	private IEnumerator PlayGame () {

		EnablePlayers();
		if (messageText != null) {
			messageText.gameObject.SetActive(false);
		}

		yield return null;
	}

	private IEnumerator EndGame () {
		yield return null;
	}

	private void SetPlayerState (bool state) {
		PlayerController[] allPlayers = GameObject.FindObjectsOfType<PlayerController>();

		foreach (PlayerController pc in allPlayers) {
			pc.enabled = state;
		}
	}

	private void EnablePlayers () {
		SetPlayerState(true);
	}

	private void DisablePlayers () {
		SetPlayerState(false);
	}
}
