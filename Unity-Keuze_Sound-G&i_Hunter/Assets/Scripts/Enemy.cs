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
	[Tooltip("Seeds")]
	public AudioClip[] Lv0_SoundTracks;
	[Tooltip("Will choose random sound from this list when the enemy is the weakest level.")]
	public AudioClip[] Lv1_SoundTracks;
	[Tooltip("Will choose random sound from this list when the enemy is the middle level.")]
	public AudioClip[] Lv2_SoundTracks;
	[Tooltip("Will choose random sound from this list when the enemy is the strongest level.")]
	public AudioClip[] Lv3_SoundTracks;
}

[System.Serializable]
public class EnemyExpRewardList {
	[Min(1)]
	public int lvl0_ExpReward;
	[Min(1)]
	public int lvl1_ExpReward;
	[Min(1)]
	public int lvl2_ExpReward;
	[Min(1)]
	public int lvl3_ExpReward;
}

public class Enemy:MonoBehaviour {
	#region Variables
	[Header("---Generated<Dont Touch>---")]
	[Tooltip("The current noice the enemy makes, after randomly being selected from the <Enemy Noices> list.")]
	[SerializeField]
	private AudioClip enemySound;

	private bool moveToPlayer, moveFromPlayer;

	//The strength of the enemy, where 1 is weakest.
	public int EnemyLevel {
		get; private set;
	}

	public int EnemyExpReward {
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
	private AudioClip enemyAttackSound;
	[Tooltip("Sound the enemy makes where the player has to determine wheter to attack or hide.")]
	[SerializeField]
	private EnemyNoices enemyNoices;
	[SerializeField]
	private EnemyExpRewardList enemyExpRewards;
	private AudioSource audioSource;


	[Header("---Enemy Visuals---")]
	[SerializeField]
	private EnemySetup enemySetup;

	private Material material;
	#endregion

	private void Awake() {
		audioSource = transform.GetComponent<AudioSource>();
		material = transform.GetComponent<Renderer>().material;
	}

	private void Update() {
		MoveCloser();
		MoveAway();
	}

	private void Movement() {
		transform.position += transform.forward * Time.deltaTime * moveSpeed;		
	}

	private void MoveCloser() {
		if(moveToPlayer) {
			Movement();

			if(Vector3.Distance(Manager.instance.player.transform.position, transform.position) <= 0.5f) {
				moveToPlayer = false;

				if(Manager.instance.playerStronger) {
					moveFromPlayer = true;
				} else {
					Attack();
				}
			}
		}
	}

	public void MoveAway() {
		if(moveFromPlayer) {
			Movement();

			if(Vector3.Distance(Manager.instance.player.transform.position, transform.position) > 5f) {
				moveFromPlayer = false;
				Manager.instance.RandomRoll();
			}
		}
	}

	//call this if the player didnt hide when the enemy is stronger
	public void Attack() {
		//moveToPlayer = false;
		//if enemy is close && stronger

		Player player = Manager.instance.player;

		if(!player.Hidden) {
			player.Attacked();
			Manager.instance.PlayClip(audioSource, enemyAttackSound);
		}
	}

	public void GetAttacked() {
		moveToPlayer = false;
		//player gains exp

		Manager.instance.RandomRoll();
	}

	public void RandomEnemy(Vector3 pos, Transform player) {
		SetStrength();
		transform.position = pos;
		transform.LookAt(player.position);
		gameObject.SetActive(true);
		moveToPlayer = true;

		Manager.instance.PlayRepeatedClip(audioSource, enemySound);
	}

	//call this on re-rolling a new enemy
	public void SetStrength() {
		EnemyLevel = RandomStrength();
		int pLvl = Manager.instance.player.playerLevel;

		if(pLvl > EnemyLevel) {
			material.color = enemySetup.weakEnemyColor;
			Manager.instance.playerStronger = true;
		} else if(pLvl < EnemyLevel) {
			material.color = enemySetup.strongEnemyColor;
			Manager.instance.playerStronger = false;
		} else {
			//shouldnt happen anymore
			material.color = enemySetup.sameEnemyColor;
		}

		GetRandomNoice();
	}

	private int RandomStrength() {
		int str;

		do {
			str = Random.Range(0, enemyMaxLevel + 1);
		} while(str == Manager.instance.player.playerLevel);

		return str;
	}

	//get noise based on strength
	private void GetRandomNoice() {
		switch(EnemyLevel) {
			case 0:
				enemySound = RandomNoiceTrack(enemyNoices.Lv0_SoundTracks);
				break;
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
				enemySound = RandomNoiceTrack(enemyNoices.Lv0_SoundTracks);
				break;
		}
	}

	private void GetExpReward() {
		switch(EnemyLevel) {
			case 0:
				EnemyExpReward = enemyExpRewards.lvl0_ExpReward;
				break;
			case 1:
				EnemyExpReward = enemyExpRewards.lvl1_ExpReward;
				break;
			case 2:
				EnemyExpReward = enemyExpRewards.lvl2_ExpReward;
				break;
			case 3:
				EnemyExpReward = enemyExpRewards.lvl3_ExpReward;
				break;
			default:
				EnemyExpReward = enemyExpRewards.lvl0_ExpReward;
				break;
		}
	}

	private AudioClip RandomNoiceTrack(AudioClip[] audioList) {
		if(audioList.Length > 0) {
			int randomNumber = Random.Range(0, audioList.Length);
			AudioClip audio = audioList[randomNumber];
			return audio;
		}

		return null;
	}
}