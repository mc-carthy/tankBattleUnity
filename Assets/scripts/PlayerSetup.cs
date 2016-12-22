using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerSetup : NetworkBehaviour {

	[SyncVarAttribute(hook="UpdateColor")]
	private Color playerColor;
	public Color PlayerColor {
		set {
			playerColor = value;
		}
	}

	[SyncVarAttribute(hook="UpdateName")]
	private string playerName = "Player";
	public string PlayerName {
		get {
			return playerName;
		}
		set {
			playerName = value;
		}
	}

	[SerializeField]
	private Text playerNameText;
	public Text PlayerNameText {
		get {
			return playerNameText;
		}
	}

	public override void OnStartClient () {
		base.OnStartClient();

		if (!isServer) {
			PlayerManager pManager = GetComponent<PlayerManager>();
			if (pManager != null) {
				GameManager.allPlayers.Add(pManager);
			}
		}
		
		UpdateName(playerName);
		UpdateColor(playerColor);
	}

	private void UpdateColor (Color pColor) {
		MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();

		foreach (MeshRenderer mesh in meshes) {
			mesh.material.color = pColor;
		}
	}

	private void UpdateName (string pName) {
		if (playerNameText != null) {
			playerNameText.enabled = true;
			playerNameText.text = pName;
		}
	}
}
