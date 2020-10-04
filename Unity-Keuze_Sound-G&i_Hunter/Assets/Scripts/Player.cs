using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//in progress
public class Player:MonoBehaviour {
	#region Variables
	//[Header("---Generated<Dont Touch>---")]
	[Header("---<Touch for debugging only>---")]
	public int playerLevel = 1;

	[Header("---Free for Editing---")]
	[Min(1)]
	[Tooltip("1 live means dying on first hit.")]
	[SerializeField]	
	private int playerMaxLives = 1;

	[Space(10)]
	[Tooltip("The eating sound the player makes when attacking.")]
	[SerializeField]
	private AudioSource attackSound;
	[Tooltip("The sound to give the player the illusion of moving forwards.")]
	[SerializeField]
	private AudioSource movementSound;
	[Tooltip("The sound reveal growth status.")]
	[SerializeField]
	private AudioSource CallSound;
	#endregion

	public int PlayerLives {
		get; private set;
	}

	private void Attack(Enemy enemy) {
		//if enemy is stronger > lose live
		//if enemy is weaker > kill enemy, growth +1
	}

	private void Hide(Enemy enemy) {
		//survive
		//??
	}

	private void Call() {
		//growth status
		//lives status?
	}


	private void Attacked() {
		PlayerLives -= 1;

		if(PlayerLives <= 0) {
			//gameover
		}
	}
}
