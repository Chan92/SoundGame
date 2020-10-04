using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySetup {
	public Color weakEnemyColor = Color.green;
	public Color strongEnemyColor = Color.red;
	public Color sameEnemyColor = Color.cyan;
}

[System.Serializable]
public class EnemyNoices {
	public AudioSource[] Lv1_SoundTracks;
	public AudioSource[] Lv2_SoundTracks;
	public AudioSource[] Lv3_SoundTracks;
}


public class Enemy:MonoBehaviour {
	[Header("---Generated<Dont Touch>---")]
	[Tooltip("The strength of the enemy, where 1 is weakest.")]
	[SerializeField]
	private int enemyLevel = 1;
	[SerializeField]
	private AudioSource enemySound;

	[Header("---Free for Editing---")]
	[Min(1)]
	[SerializeField]
	private int enemyMaxLevel = 3;
	[SerializeField]
	private float moveSpeed = 10f;
	[Space(10)]
	[Tooltip("The eating sound the enemy makes.")]
	[SerializeField]
	private AudioSource enemyAttack;
	[Tooltip("Will choose random sound from this list.")]
	[SerializeField]
	private EnemyNoices enemyNoices;

	[Header("---Enemy Visuals---")]
	[SerializeField]
	private EnemySetup enemySetup;

	private Player player;
	private Material material;

	private void Awake() {
		player = Transform.FindObjectOfType<Player>();
		material = transform.GetComponent<Renderer>().material;
	}

	private void Update() {
		Movement();
	}

	private void Movement() {
		transform.LookAt(player.transform);
		transform.position += transform.forward * Time.deltaTime * moveSpeed;
	}

	public void SetStrength() {
		enemyLevel = Random.Range(1, enemyMaxLevel +1);

		if(player.playerLevel > enemyLevel) {
			material.color = enemySetup.weakEnemyColor;
		} else if(player.playerLevel < enemyLevel) {
			material.color = enemySetup.strongEnemyColor;
		} else {
			material.color = enemySetup.sameEnemyColor;
		}

		GetRandomNoice();
	}

	//get noise based on strength
	public void GetRandomNoice() {
		switch(enemyLevel) {
			case 1:
				enemySound = RandomNoiceTrack(enemyNoices.Lv1_SoundTracks);
				break;
			case 2:
				enemySound = RandomNoiceTrack(enemyNoices.Lv2_SoundTracks);
				break;
			case 3:
				enemySound = RandomNoiceTrack(enemyNoices.Lv3_SoundTracks);
				break;
			default:
				enemySound = RandomNoiceTrack(enemyNoices.Lv1_SoundTracks);
				break;
		}
	}

	private AudioSource RandomNoiceTrack(AudioSource[] audioList) {
		if(audioList.Length > 0) {
			int randomNumber = Random.Range(0, audioList.Length);
			AudioSource audio = audioList[randomNumber];
			return audio;
		}

		return null;
	}
}
