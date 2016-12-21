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
	private int playerNum;
	public int PlayerNum {
		set {
			playerNum = value;
		}
	}

	[SerializeField]
	private Text playerNameText;

	private string baseName = "Player";

	private void Start () {
		if (!isLocalPlayer) {
			UpdateName(playerNum);
			UpdateColor(playerColor);
		}
	}

	public override void OnStartClient () {
		base.OnStartClient();

		if (playerNameText != null) {
			playerNameText.enabled = false;
		}
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

	private void UpdateName (int pNum) {
		if (playerNameText != null) {
			playerNameText.enabled = true;
			playerNameText.text = baseName + pNum.ToString();
		}
	}

	[CommandAttribute]
	private void CmdSetupPlayer () {
		GameManager.Instance.AddPlayer(this);
	}
}
