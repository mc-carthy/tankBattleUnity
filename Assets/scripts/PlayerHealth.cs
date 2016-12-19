using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : NetworkBehaviour {

	[SyncVar(hook="UpdateHealthBar")]
	private float currentHealth;
	
	[SerializeField]
	private GameObject deathPrefab;
	[SerializeField]
	private RectTransform healthBar;

	private float maxHealth = 3f;
	private bool isDead;


	private void Start () {
		currentHealth = maxHealth;
	}

	public void Damage (float damage) {

		if (!isServer) {
			return;
		}

		currentHealth -= damage;
		if (currentHealth <= 0 && !isDead) {
			isDead = true;
			Die();
		}
	}

	private void Die () {
		if (deathPrefab != null) {
			GameObject deathFx = Instantiate(deathPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity) as GameObject;
			Destroy(deathFx.gameObject, 3f);
		}

		SetActiveState(false);

		gameObject.SendMessage("Disable");
	}

	private void SetActiveState (bool state) {
		foreach (Collider c in GetComponentsInChildren<Collider>()) {
			c.enabled = state;
		}

		foreach (Canvas c in GetComponentsInChildren<Canvas>()) {
			c.enabled = state;
		}

		foreach (Renderer r in GetComponentsInChildren<Renderer>()) {
			r.enabled = state;
		}
	}

	private void UpdateHealthBar (float value) {
		if (healthBar != null) {
			healthBar.sizeDelta = new Vector2(value / maxHealth * healthBar.rect.width, healthBar.sizeDelta.y);
		}
	}
}
