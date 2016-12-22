using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class NetworkLobbyHook : LobbyHook {

	public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer) {
		LobbyPlayer lPlayer = lobbyPlayer.GetComponent<LobbyPlayer>();
		PlayerSetup pSetup = gamePlayer.GetComponent<PlayerSetup>();

		pSetup.PlayerName = lPlayer.playerName;
		pSetup.PlayerColor = lPlayer.playerColor;
	}

}
