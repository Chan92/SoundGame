using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player:MonoBehaviour {
	[Header("Player Stats")]
	[SerializeField]
	public int playerLevel = 1;
	private int playerMaxLives = 1;


	public int PlayerLives {
		get; private set;
	}

	private void Attack(Enemy enemy) {

	}

	private void Hide(Enemy enemy) {

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
