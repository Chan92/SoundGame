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
	[Tooltip("Will choose random sound from this list when the enemy is the weakest level.")]
	public AudioSource[] Lv1_SoundTracks;
	[Tooltip("Will choose random sound from this list when the enemy is the middle level.")]
	public AudioSource[] Lv2_SoundTracks;
	[Tooltip("Will choose random sound from this list when the enemy is the strongest level.")]
	public AudioSource[] Lv3_SoundTracks;
}


public class Enemy:MonoBehaviour {
	#region Variables
	[Header("---Generated<Dont Touch>---")]
	[Tooltip("The current noice the enemy makes, after randomly being selected from the <Enemy Noices> list.")]
	[SerializeField]
	private AudioSource enemySound;

	//The strength of the enemy, where 1 is weakest.
	public int EnemyLevel {
		get; private set;
	}
	

	[Header("---Free for Editing---")]
	[Min(1)]
	[SerializeField]
	private int enemyMaxLevel = 3;
	[SerializeField]
	private float moveSpeed = 5f;

	[Space(10)]
	[Tooltip("The eating sound the enemy makes when the player is weaker and didnt hide.")]
	[SerializeField]
	private AudioSource enemyAttackSound;
	[Tooltip("Sound the enemy makes where the player has to determine wheter to attack or hide.")]
	[SerializeField]
	private EnemyNoices enemyNoices;


	[Header("---Enemy Visuals---")]
	[SerializeField]
	private EnemySetup enemySetup;

	private Material material;
	#endregion

	private void Awake() {
		material = transform.GetComponent<Renderer>().material;
	}

	private void Update() {
		Movement();
	}

	private void Movement() {
		//transform.LookAt(player.transform);
		transform.position += transform.forward * Time.deltaTime * moveSpeed;
	}

	//call this if the player didnt hide when the enemy is stronger
	public void Attack() {
		if(!Manager.instance.player.Hidden) {
			//attacks the player
			//attack noice
		}
	}

	//call this on re-rolling a new enemy
	public void SetStrength() {
		EnemyLevel = Random.Range(1, enemyMaxLevel +1);

		if(Manager.instance.player.playerLevel > EnemyLevel) {
			material.color = enemySetup.weakEnemyColor;
		} else if(Manager.instance.player.playerLevel < EnemyLevel) {
			material.color = enemySetup.strongEnemyColor;
		} else {
			material.color = enemySetup.sameEnemyColor;
		}

		GetRandomNoice();
	}

	//get noise based on strength
	private void GetRandomNoice() {
		switch(EnemyLevel) {
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