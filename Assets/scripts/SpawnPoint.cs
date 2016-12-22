using UnityEngine;

public class SpawnPoint : MonoBehaviour {

	private bool isOccupied;
	public bool IsOccupied {
		get {
			return isOccupied;
		}
		set {
			isOccupied = value;
		}
	}

	private void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			isOccupied = true;
		}
	}

	private void OnTriggerStay (Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			isOccupied = true;
		}
	}

	private void OnTriggerExit (Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			isOccupied = false;
		}
	}

}
