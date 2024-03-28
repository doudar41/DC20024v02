//==============================================================
// Stairs (C) 2024 Zombisoft
// Go down and up in the Dungeon.
//==============================================================

using System.Collections;
using UnityEngine;

namespace zombisoft
{
	public class Stairs : MonoBehaviour
	{
		//==============================================================
		//   Variables
		//==============================================================
		[Header("Stairs Target")]
		public GameObject Target;

		private Transform Player;

		//==============================================================
		// Start
		//==============================================================	
		void Start()
		{
			Player = GameObject.FindWithTag("Player").transform; // Cache the player transform
		}

		//==============================================================
		// On Trigger Enter
		//==============================================================
		protected void OnTriggerEnter(Collider t)
		{
			StartCoroutine("ChangeLevel");
		}

		//==============================================================
		// Coroutine Change Level
		//==============================================================
		IEnumerator ChangeLevel()
		{
			DungeonCrawlerController.freezePlayerInput = true; // Freeze player
			Fader.FadeIn(Color.black, 0.5f);
			yield return new WaitForSeconds(0.5f);
			Player.transform.position = Target.transform.position; // Move player to the target position
			Fader.FadeOut(Color.black, 0.5f);
			yield return null;
			DungeonCrawlerController.freezePlayerInput = false; // Unfreeze player
		}
	}
}