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
	private AudioClip attackSound;
	[Tooltip("The sound to give the player the illusion of moving forwards.")]
	//[SerializeField]
	private AudioClip movementSound;
	[Tooltip("The sound reveal growth status.")]
	[SerializeField]
	private AudioClip callSound;
	[SerializeField]
	private AudioClip gameOverSound;

	private AudioSource audioSource;
	#endregion

	private void Awake() {
		audioSource = transform.GetComponent<AudioSource>();
	}

	private void Update() {
		Attack();
		Hide();
	}

	private void Attack() {
		Attacking = Input.GetKey(attackKey);

		if(Attacking) {			
			Enemy enemy = Manager.instance.enemy;

			if(Vector3.Distance(transform.position, enemy.transform.position) < attackDistance) {
				attackDebugText.SetActive(Attacking);
				int enemyLevel = enemy.EnemyLevel;
				Manager.instance.PlayClip(audioSource, attackSound);

				if(enemyLevel > playerLevel) {
					Attacked();
				//same lvl is removed for now
				//} else if(enemyLevel == playerLevel) {			
					//same level, ???
				} else {
					UpdateLevel(enemy.EnemyExpReward);
					Manager.instance.enemy.GetAttacked();
				}
			}
		} else if(Input.GetKeyUp(attackKey)) {
			attackDebugText.SetActive(Attacking);
		}
	}

	//TODO: hide sound?
	private void Hide() {
		Hidden = Input.GetKey(hideKey);

		if(Hidden) {
			Enemy enemy = Manager.instance.enemy;

			if(Vector3.Distance(transform.position, enemy.transform.position) < attackDistance) {
				hideDebugText.SetActive(Hidden);
				Manager.instance.enemy.MoveAway();
			}
		} else if(Input.GetKeyUp(hideKey)) {
			hideDebugText.SetActive(Hidden);
		}
	}

	//TODO: create/finish call function
	private void Call() {
		if(Input.GetKeyDown(callKey)) {
			Manager.instance.PlayClip(audioSource, callSound);
			//growth status
			//lives status?
		}
	}

	//TODO: finish win game
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
				if(playerExp >= expToLevelUp.toWinGame) {
					playerLevel++;
					playerExp = 0;
					//win game
				}
				break;
		}
	}
	
	//TODO: finish gameover
	public void Attacked() {
		PlayerLives -= 1;

		if(PlayerLives <= 0) {
			Manager.instance.PlayClip(audioSource, gameOverSound);
		}

		Manager.instance.RandomRoll();
	}
}
