using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Prototype.NetworkLobby;

public class GameManager : NetworkBehaviour {

	[SyncVarAttribute]
	private bool isGameOver;

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

	public static List<PlayerManager> allPlayers = new List<PlayerManager>();

	[SerializeField]
	private Text messageText;	
	[SerializeField]
	private List<Text> playerNameText;
	[SerializeField]
	private List<Text> playerScoreText;

	private PlayerManager winner;
	private int maxScore = 3;

	private void Awake () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}

	[ServerAttribute]
	private void Start () {
		StartCoroutine(GameLoopRoutine());
	}

	private void CheckScores () {
		winner = GetWinner();
		if (winner != null) {
			isGameOver = true;
		}
	}

	public void UpdateMessage (string msg) {
		if (messageText != null) {
			messageText.gameObject.SetActive(true);
			messageText.text = msg;
		}
	}

	[ServerAttribute]
	public void UpdateScoreboard () {
		string[] pNames = new string[allPlayers.Count];
		int[] pScores = new int[allPlayers.Count];

		Debug.Log(allPlayers.Count);

		for (int i = 0; i < allPlayers.Count; i++) {
			if (allPlayers[i] != null) {
				pNames[i] = allPlayers[i].GetComponent<PlayerSetup>().PlayerName;
				pScores[i] = allPlayers[i].Score;
			}
		}

		RpcUpdateScoreboard(pNames, pScores);
	}

	public void Reset () {
		for (int i = 0; i < allPlayers.Count; i++) {
			PlayerHealth pHealth = allPlayers[i].GetComponent<PlayerHealth>();
			pHealth.Reset();

			allPlayers[i].Score = 0;
		}
	}

	private IEnumerator GameLoopRoutine() {

		LobbyManager lobbyManager = LobbyManager.s_Singleton;

		if (lobbyManager != null) {
			while (allPlayers.Count < lobbyManager._playerNumber) {
				yield return null;
			}

			yield return new WaitForSeconds(2f);

			yield return StartCoroutine(StartGame());
			yield return StartCoroutine(PlayGame());
			yield return StartCoroutine(EndGame());
			StartCoroutine(GameLoopRoutine());
		} else {
			Debug.LogWarning("GameManager warning, launch scene from lobby scene only!");
		}
	}


	private IEnumerator StartGame () {
		Reset();
		RpcStartGame();
		UpdateScoreboard();
		yield return new WaitForSeconds(3f);
	}

	private IEnumerator PlayGame () {

		yield return new WaitForSeconds(1f);

		RpcPlayGame();

		while (isGameOver == false) {
			CheckScores();
			yield return null;
		}
	}

	private IEnumerator EndGame () {
		RpcEndGame();
		RpcUpdateMessage("GAME OVER\n" + winner.PSetup.PlayerNameText.text + " wins!");
		yield return new WaitForSeconds(3f);
		Reset();

		LobbyManager.s_Singleton._playerNumber = 0;
		LobbyManager.s_Singleton.SendReturnToLobby();
	}

	private void EnablePlayers () {
		for (int i = 0; i < allPlayers.Count; i++) {
			if (allPlayers[i] != null) {
				allPlayers[i].EnableControls();
			}
		}
	}

	private void DisablePlayers () {
		for (int i = 0; i < allPlayers.Count; i++) {
			if (allPlayers[i] != null) {
				allPlayers[i].DisableControls();
			}
		}
	}

	[ClientRpcAttribute]
	private void RpcUpdateScoreboard (string[] playerNames, int[] playerScores) {
		for (int i = 0; i < allPlayers.Count; i++) {
			if (playerNameText[i] != null) {
				playerNameText[i].text = playerNames[i];
			}
			if (playerScoreText[i] != null) {
				playerScoreText[i].text = playerScores[i].ToString();
			}
		}
	}

	[ClientRpcAttribute]
	private void RpcUpdateMessage(string msg) {
		UpdateMessage(msg);
	}

	[ClientRpcAttribute]
	private void RpcStartGame() {
		UpdateMessage("Fight!");
		DisablePlayers();
	}

	[ClientRpcAttribute]
	private void RpcPlayGame() {
		EnablePlayers();
		UpdateMessage("");
	}

	[ClientRpcAttribute]
	private void RpcEndGame() {
		DisablePlayers();
	}

	private PlayerManager GetWinner () {
		for (int i = 0; i < allPlayers.Count; i++)
		{
			if (allPlayers[i].Score >= maxScore) {
				return allPlayers[i];
			}
		}
	return null;
	}
}
