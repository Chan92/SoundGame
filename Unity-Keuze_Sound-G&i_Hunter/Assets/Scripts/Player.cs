using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExpToLevelUp {
	[Min(1)]
	public int toLevel2;
	[Min(1)]
	public int toLevel3;
	[Min(1)]
	public int toLevel4;
	[Min(1)]
	public int toLevel5;
	[Min(1)]
	public int toWinGame;
}

public class Player:MonoBehaviour {
	#region Variables
	public GameObject hideDebugText, attackDebugText;

	[Header("---<Touch for debugging only>---")]
	public int playerLevel = 1;
	private int playerExp;

	public int PlayerLives {
		get; private set;
	}

	public bool Hidden {
		get; private set;
	}

	public bool Attacking {
		get; private set;
	}

	[Header("---Free for Editing---")]
	[SerializeField]
	private KeyCode hideKey = KeyCode.Space;
	[SerializeField]
	private KeyCode attackKey = KeyCode.Mouse0;
	[SerializeField]
	private KeyCode callKey = KeyCode.Mouse1;

	[Space(10)]
	[Min(1)]
	[Tooltip("1 live means dying on first hit.")]
	[SerializeField]	
	private int playerMaxLives = 1;
	[SerializeField]
	private float attackDistance = 3f;
	[SerializeField]
	private ExpToLevelUp expToLevelUp;

	[Space(10)]
	[Tooltip("The eating sound the player makes when attacking.")]
	[SerializeField]
	private AudioClip[] attackSound = new AudioClip[5];
	[Tooltip("The sound to give the player the illusion of moving forwards.")]
	[SerializeField]
	private AudioClip[] movementSound = new AudioClip[5];
	[SerializeField]
	private AudioSource movementSource;
	[Tooltip("The sound reveal growth status.")]
	[SerializeField]
	private AudioClip hideSound;
	[SerializeField]
	private AudioClip[] callSound = new AudioClip[5];
	[SerializeField]
	private AudioClip gameOverSound, winSound;

	private AudioSource audioSource;
	#endregion

	private void Awake() {
		audioSource = transform.GetComponent<AudioSource>();
		hideDebugText.SetActive(false);
		attackDebugText.SetActive(false);
	}

	private void Update() {
		Attack();
		Hide();
		Call();
	}

	private void Attack() {
		if(Input.GetKeyDown(attackKey)) {
			StartCoroutine(AttackEnemy());
		}
	}

	private void Hide() {
		if(Input.GetKeyDown(hideKey)) {
			StartCoroutine(Hidding());		
		}
	}

	private void Call() {
		if(Input.GetKeyDown(callKey)) {
			Manager.instance.PlayClip(audioSource, callSound[playerLevel - 1]);
		}
	}

	//TODO: win game sound
	private void UpdateLevel(int amount) {
		playerExp += amount;

		switch(playerLevel) {
			case 1:
				if(playerExp >= expToLevelUp.toLevel2) {
					playerLevel++;
					playerExp = 0;
				}
				break;
			case 2:
				if(playerExp >= expToLevelUp.toLevel3) {
					playerLevel++;
					playerExp = 0;
				}
				break;
			case 3:
				if(playerExp >= expToLevelUp.toLevel4) {
					playerLevel++;
					playerExp = 0;
				}
				break;
			case 4:
				if(playerExp >= expToLevelUp.toLevel5) {
					playerLevel++;
					playerExp = 0;
				}
				break;
			case 5:
				if(playerExp >= expToLevelUp.toWinGame) {
					Manager.instance.gameWin = true;
					playerLevel++;
					playerExp = 0;
					Manager.instance.PlayClip(audioSource, winSound);
					Manager.instance.ReloadLevel();
				}
				break;
		}
	}
	
	public void Attacked() {
		PlayerLives -= 1;

		if(PlayerLives <= 0) {
			Manager.instance.PlayClip(audioSource, gameOverSound);
			Manager.instance.ReloadLevel();
		} else {
			Manager.instance.RandomRoll();
		}
	}

	public void FootStepsSound() {
		if(playerLevel <= movementSound.Length) {
			Manager.instance.PlayRepeatedClip(movementSource, movementSound[playerLevel-1]);
		}
	}

	IEnumerator Hidding() {
		Hidden = true;
		Enemy enemy = Manager.instance.enemy;

		while (Vector3.Distance(transform.position, enemy.transform.position) > attackDistance) {
			yield return new WaitForSeconds(0);
		}

		movementSource.Stop();
		hideDebugText.SetActive(Hidden);
		Manager.instance.PlayClip(audioSource, hideSound);
		Manager.instance.enemy.LostTarget();

		yield return new WaitForSeconds(1.5f);
		Hidden = false;
		hideDebugText.SetActive(Hidden);
	}

	IEnumerator AttackEnemy() {
		Attacking = true;
		Enemy enemy = Manager.instance.enemy;

		while(Vector3.Distance(transform.position, enemy.transform.position) > attackDistance) {
			yield return new WaitForSeconds(0);
		}

		attackDebugText.SetActive(Attacking);
		int enemyLevel = enemy.EnemyLevel;
		Manager.instance.PlayClip(audioSource, attackSound[playerLevel - 1]);

		if(enemyLevel > playerLevel) {
			Attacked();
		} else {
			UpdateLevel(enemy.EnemyExpReward);
			Manager.instance.enemy.GetAttacked();
		}

		yield return new WaitForSeconds(1.5f);
		Attacking = false;
		attackDebugText.SetActive(Attacking);
	}
}
