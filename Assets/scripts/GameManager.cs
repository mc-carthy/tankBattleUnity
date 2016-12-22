using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : NetworkBehaviour {

	[SyncVarAttribute]
	private int playerCount = 0;
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

	public List<PlayerManager> allPlayers; // TODO - Only works when public, not whem private or a property, fix

	[SerializeField]
	private Text messageText;	
	[SerializeField]
	private List<Text> nameLabelText;
	[SerializeField]
	private List<Text> playerScoreText;

	private Color[] playerColors = { Color.red, Color.blue, Color.green, Color.yellow };
	private PlayerManager winner;
	private int minPlayers = 1;
	private int maxPlayers = 4;
	private int maxScore = 3;

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
			allPlayers.Add(pSetup.GetComponent<PlayerManager>());
			pSetup.PlayerColor = playerColors[playerCount];
			playerCount++;
			//pSetup.PlayerNum = playerCount;
		}
	}

	public void UpdateScoreboard () {
		if (isServer) {

			winner = GetWinner();
			if (winner != null) {
				isGameOver = true;
			}

			string[] names = new string[playerCount];
			int[] scores = new int[playerCount];

			for (int i = 0; i < playerCount; i++) {
				names[i] = allPlayers[i].GetComponent<PlayerSetup>().PlayerNameText.text;
				scores[i] = allPlayers[i].Score;
			}

			RpcUpdateScoreboard(names, scores);
		}
	}

	public void UpdateMessage (string msg) {
		if (isServer) {
			RpcUpdateMessage(msg);
		}
	}

	public void Reset () {
		if (isServer) {
			RpcReset();
			UpdateScoreboard();
			winner = null;
			isGameOver = false;
		}
	}

	private IEnumerator GameLoopRoutine() {
		yield return StartCoroutine(EnterLobby());
		yield return StartCoroutine(PlayGame());
		yield return StartCoroutine(EndGame());
		StartCoroutine(GameLoopRoutine());
	}

	private IEnumerator EnterLobby () {
		
		while (playerCount < minPlayers) {
			UpdateMessage("Waiting for other players...");
			DisablePlayers();
			yield return null;
		}
	}

	private IEnumerator PlayGame () {

		yield return new WaitForSeconds(2f);
		UpdateMessage("3");
		yield return new WaitForSeconds(1f);
		UpdateMessage("2");
		yield return new WaitForSeconds(1f);
		UpdateMessage("1");
		yield return new WaitForSeconds(1f);
		UpdateMessage("Fight!");

		EnablePlayers();
		UpdateScoreboard();

		UpdateMessage("");

		PlayerManager winner = null;
		while (isGameOver == false) {
			yield return null;
		}
	}

	private IEnumerator EndGame () {
		DisablePlayers();

		UpdateMessage("GAME OVER\n" + winner.PSetup.PlayerNameText.text + " wins!");

		Reset();

		yield return new WaitForSeconds(3f);
		UpdateMessage("Starting new game...");
		yield return new WaitForSeconds(3f);
	}

	[ClientRpcAttribute]
	private void RpcSetPlayerState (bool state) {
		PlayerManager[] allPlayers = GameObject.FindObjectsOfType<PlayerManager>();

		foreach (PlayerManager pc in allPlayers) {
			pc.enabled = state;
		}
	}

	private void EnablePlayers () {
		if (isServer) {
			RpcSetPlayerState(true);
		}
	}

	private void DisablePlayers () {
		if (isServer) {
				RpcSetPlayerState(false);
		}
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

	[ClientRpcAttribute]
	private void RpcUpdateMessage(string msg) {
		if (messageText != null) {
			messageText.gameObject.SetActive(true);
			messageText.text = msg;
		}
	}

	[ClientRpcAttribute]
	private void RpcReset () {
		PlayerManager[] allPlayers = GameObject.FindObjectsOfType<PlayerManager>();
		
		foreach (PlayerManager pc in allPlayers) {
			pc.Score = 0;
		}
	}

	private PlayerManager GetWinner () {
		if (isServer) {
			for (int i = 0; i < playerCount; i++)
			{
				if (allPlayers[i].Score >= maxScore) {
					return allPlayers[i];
				}
			}
		}
		return null;
	}
}
