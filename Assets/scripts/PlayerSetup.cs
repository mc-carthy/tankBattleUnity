using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	private Color playerColor;
	[SerializeField]
	private string baseName = "Player";
	[SerializeField]
	private int playerNum = 1;
	[SerializeField]
	private Text playerNameText;

	public override void OnStartClient () {
		base.OnStartClient();

		if (playerNameText != null) {
			playerNameText.enabled = false;
		}
	}

	public override void OnStartLocalPlayer () {
		base.OnStartLocalPlayer();

		MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();

		foreach (MeshRenderer mesh in meshes) {
			mesh.material.color = playerColor;
		}

		if (playerNameText != null) {
			playerNameText.enabled = true;
			playerNameText.text = baseName + playerNum.ToString();
		}
	}


}
