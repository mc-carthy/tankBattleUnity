using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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

	public List<PlayerController> allPlayers; // TODO - Only works when public, not whem private or a property, fix

	[SerializeField]
	private Text messageText;	
	[SerializeField]
	private List<Text> nameLabelText;
	[SerializeField]
	private List<Text> playerScoreText;

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
			Debug.Log(pSetup.gameObject.GetComponent<PlayerController>());
			allPlayers.Add(pSetup.GetComponent<PlayerController>());
			pSetup.PlayerColor = playerColors[playerCount];
			playerCount++;
			pSetup.PlayerNum = playerCount;
		}
	}

	public void UpdateScoreboard () {
		if (isServer) {
			string[] names = new string[playerCount];
			int[] scores = new int[playerCount];

			for (int i = 0; i < playerCount; i++) {
				names[i] = allPlayers[i].GetComponent<PlayerSetup>().PlayerNameText.text;
				scores[i] = allPlayers[i].Score;
			}

			RpcUpdateScoreboard(names, scores);
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
		UpdateScoreboard();
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

	[ClientRpcAttribute]
	private void RpcUpdateScoreboard (string[] playerNames, int[] playerScores) {
		for (int i = 0; i < playerCount; i++) {
			if (nameLabelText[i] != null) {
				nameLabelText[i].text = playerNames[i];
			}
			if (playerScoreText[i] != null) {
				playerScoreText[i].text = playerScores[i].ToString();
			}
		}
	}
}
