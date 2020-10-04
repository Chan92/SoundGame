using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//in progress
public class Manager : MonoBehaviour {
	#region Variables
	[Header("---<Debugging tools>---")]
	[SerializeField]
	private bool useBlackScreen;
	[SerializeField]
	private GameObject blackScreen;

	[Header("---Free for Editing---")]
	[SerializeField]
	private float minSpawnDistance = 5f;
	[SerializeField]
	private float maxSpawnDistance = 15f;

	private Enemy enemy;
	private Player player;
	#endregion

	private void Start() {
		enemy = Transform.FindObjectOfType<Enemy>();
		player = Transform.FindObjectOfType<Player>();
		enemy.gameObject.SetActive(false);

		blackScreen.SetActive(useBlackScreen);
	}

	private void Update() {
		//debugging
		if(Input.GetButtonDown("Jump"))
			RandomRoll();
	}

	public void RandomRoll() {
		enemy.SetStrength();
		enemy.transform.position = RandomPosition();
		enemy.transform.LookAt(player.transform);
		enemy.gameObject.SetActive(true);
	}

	private Vector3 RandomPosition() {
		Vector3 pos = Vector3.zero;
		pos.x = Random.Range(minSpawnDistance, maxSpawnDistance);
		pos.z = Random.Range(minSpawnDistance, maxSpawnDistance);

		//randomly change value between positive and negative
		pos.x *= RandomSign();
		pos.y *= RandomSign();

		return pos;
	}

	private float RandomSign() {
		return Random.value < 0.5f ? 1 : -1;
	}
}
