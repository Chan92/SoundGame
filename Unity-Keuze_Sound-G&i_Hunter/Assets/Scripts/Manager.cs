using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//in progress
public class Manager : MonoBehaviour {
	#region Variables
	public static Manager instance;

	[Header("---<Debugging tools>---")]
	[SerializeField]
	private bool useBlackScreen;
	[SerializeField]
	private GameObject blackScreen;
	[SerializeField]
	private Text playerLevelTxt, playerLivesTxt, enemyLevelTxt;
	

	[Header("---Free for Editing---")]
	[SerializeField]
	private float minSpawnDistance = 5f;
	[SerializeField]
	private float maxSpawnDistance = 15f;

	public Enemy enemy;
	public Player player;
	#endregion

	private void Awake() {
		instance = this;
	}

	private void Start() {
		enemy = Transform.FindObjectOfType<Enemy>();
		player = Transform.FindObjectOfType<Player>();
		enemy.gameObject.SetActive(false);

		blackScreen.SetActive(useBlackScreen);
		RandomRoll();
	}

	private void Update() {
		//debugging
		if(Input.GetKeyDown(KeyCode.N))
			RandomRoll();
	}

	public void RandomRoll() {
		enemy.SetStrength();
		enemy.transform.position = RandomPosition();
		enemy.transform.LookAt(player.transform);
		enemy.gameObject.SetActive(true);

		if(!useBlackScreen) UpdateUi();
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

	private void UpdateUi (){
		playerLevelTxt.text = "Player Level: " + player.playerLevel;
		playerLivesTxt.text = "Player Lives: " + player.PlayerLives;
		enemyLevelTxt.text = enemy.EnemyLevel + " :Enemy Level";
	}
}
