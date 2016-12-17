using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	private Color playerColor;

	public override void OnStartLocalPlayer () {
		MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();

		foreach (MeshRenderer mesh in meshes) {
			mesh.material.color = playerColor;
		}
	}


}
