using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Manager : MonoBehaviour {
	[SerializeField]
	private Enemy enemy;
	[SerializeField]
	private float minSpawnDistance = 5f, maxSpawnDistance = 15f;

	private void Start() {
		enemy.gameObject.SetActive(false);
	}

	private void Update() {
		//debugging
		if(Input.GetButtonDown("Jump"))
			RandomRoll();
	}

	public void RandomRoll() {
		enemy.SetStrength();
		enemy.transform.position = RandomPosition();
		enemy.gameObject.SetActive(true);
	}

	private Vector3 RandomPosition() {
		Vector3 pos = Vector3.zero;
		pos.x = Random.Range(minSpawnDistance, maxSpawnDistance);
		pos.z = Random.Range(minSpawnDistance, maxSpawnDistance);

		return pos;
	}
}
