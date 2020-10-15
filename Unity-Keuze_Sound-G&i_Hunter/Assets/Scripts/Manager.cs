using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour {
	#region Variables
	public static Manager instance;
	[HideInInspector]
	public bool playerStronger;

	[Header("---<Debugging tools>---")]
	[SerializeField]
	private GameObject startGameDebugText;
	[SerializeField]
	private bool useBlackScreen;
	[SerializeField]
	private GameObject blackScreen;
	[SerializeField]
	private Text playerLevelTxt, playerLivesTxt, enemyLevelTxt;
	public bool gameWin;


	[Header("---Free for Editing---")]
	[SerializeField]
	private float respawnDelay = 1.5f;
	[SerializeField]
	private float minSpawnDistance = 5f;
	[SerializeField]
	private float maxSpawnDistance = 15f;
	[SerializeField]
	private KeyCode startGameKey, randomRollKey, blackScreenKey;

	public Enemy enemy;
	public Player player;

	[HideInInspector]
	public bool gameStarted = false;
	#endregion

	private void Awake() {
		instance = this;
	}

	private void Start() {
		gameWin = false;
		startGameDebugText.SetActive(true);
		enemy = Transform.FindObjectOfType<Enemy>();
		player = Transform.FindObjectOfType<Player>();
		enemy.gameObject.SetActive(false);

		blackScreen.SetActive(useBlackScreen);
	}

	private void Update() {
		if(Input.GetKeyDown(startGameKey) && !gameStarted) {
			startGameDebugText.SetActive(false);
			gameStarted = true;
			RandomRoll();
		}
		
		//debugging
		if(Input.GetKeyDown(randomRollKey) && gameStarted) {
			RandomRoll();
		}

		if(Input.GetKeyDown(blackScreenKey)) {
			useBlackScreen = !useBlackScreen;
			blackScreen.SetActive(useBlackScreen);
		}
	}

	public void RandomRoll() {
		enemy.RandomEnemy(RandomPosition(), player.transform);
		UpdateUi();
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

	public void PlayClip(AudioSource audioSource, AudioClip audioClip) {
		if(audioSource && audioClip) {
			audioSource.Stop();
			audioSource.loop = false;
			audioSource.PlayOneShot(audioClip);
		}
	}

	public void PlayRepeatedClip(AudioSource audioSource, AudioClip audioClip) {
		if(audioSource && audioClip) {
			audioSource.Stop();
			audioSource.loop = true;
			audioSource.clip = audioClip;
			audioSource.Play();
		}
	}

	public void ReloadLevel() {
		StartCoroutine(Reload());
	}

	private IEnumerator Reload() {
		yield return new WaitForSeconds(respawnDelay);

		SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
		Resources.UnloadUnusedAssets();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
	}
}
