using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

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

	private void Awake () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}
}
