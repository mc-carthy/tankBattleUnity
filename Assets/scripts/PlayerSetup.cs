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

	// private int playerNum;
	// public int PlayerNum {
	// 	set {
	// 		playerNum = value;
	// 	}
	// }

	[SerializeField]
	private Text playerNameText;
	public Text PlayerNameText {
		get {
			return playerNameText;
		}
	}

	private void Start () {
		UpdateName(playerName);
		UpdateColor(playerColor);
	}

	public override void OnStartClient () {
		base.OnStartClient();
	}

	public override void OnStartLocalPlayer () {
		base.OnStartLocalPlayer();
		CmdSetupPlayer();
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

	[CommandAttribute]
	private void CmdSetupPlayer () {
		GameManager.Instance.AddPlayer(this);
	}
}
