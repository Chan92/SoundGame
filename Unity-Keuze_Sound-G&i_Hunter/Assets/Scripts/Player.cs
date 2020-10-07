using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//in progress
public class Player:MonoBehaviour {
	#region Variables
	//[Header("---Generated<Dont Touch>---")]
	public GameObject hideDebugText;

	[Header("---<Touch for debugging only>---")]
	public int playerLevel = 1;

	public int PlayerLives {
		get; private set;
	}

	public bool Hidden {
		get; private set;
	}

	[Header("---Free for Editing---")]
	[SerializeField]
	private KeyCode hideKey = KeyCode.Space;
	[SerializeField]
	private KeyCode attackKey = KeyCode.Mouse0;
	[SerializeField]
	private KeyCode callKey = KeyCode.Mouse1;

	[Min(1)]
	[Tooltip("1 live means dying on first hit.")]
	[SerializeField]	
	private int playerMaxLives = 1;
	[SerializeField]
	private float attackDistance = 3f;

	[Space(10)]
	[Tooltip("The eating sound the player makes when attacking.")]
	[SerializeField]
	private AudioClip attackSound;
	[Tooltip("The sound to give the player the illusion of moving forwards.")]
	[SerializeField]
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
		if(Input.GetKeyDown(attackKey)) {
			Enemy enemy = Manager.instance.enemy;

			if(Vector3.Distance(transform.position, enemy.transform.position) < attackDistance) {
				int enemyLevel = enemy.EnemyLevel;
				PlayClip(attackSound);
				
				if(enemyLevel > playerLevel) {
					Attacked();
				//} else if(enemyLevel == playerLevel) {			
					//same level, ???
				} else {
					UpdateLevel(1);
					Manager.instance.RandomRoll();
				}
			}
		}
	}

	private void Hide() {
		Hidden = Input.GetKey(hideKey);
		hideDebugText.SetActive(Hidden);

		//...
		//Manager.instance.RandomRoll();
	}

	private void Call() {
		if(Input.GetKeyDown(callKey)) {
			PlayClip(callSound);
			//growth status
			//lives status?
		}
	}

	private void UpdateLevel(int amount) {
		playerLevel += amount;
	}
	
	public void Attacked() {
		PlayerLives -= 1;

		if(PlayerLives <= 0) {
			PlayClip(gameOverSound);
		}

		Manager.instance.RandomRoll();
	}

	private void PlayClip(AudioClip audioClip) {
		if(audioSource && audioClip)
			audioSource.PlayOneShot(audioClip);
	}
}
