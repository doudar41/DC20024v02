//==============================================================
// PressurePlate (C) 2024 Zombisoft
// Attach to a pressurePlate
//==============================================================

using System.Collections;
using UnityEngine;

namespace zombisoft
{
	public class PressurePlate : MonoBehaviour
	{
		private GameObject Camera; // Camera slightly move down when stepping on stone

		//==============================================================
		// Start
		//==============================================================
		private void Start()
		{
			Camera = GameObject.FindGameObjectWithTag("MainCamera");
		}

		//==============================================================
		// On Trigger Enter
		//==============================================================
		protected void OnTriggerEnter(Collider t)
		{
			StartCoroutine(RotateCamera(0.2f)); // Pressure Plate
		}

		//==============================================================
		// Coroutine: Rotate Camera slightly Down and Up
		//==============================================================
		IEnumerator RotateCamera(float duration)
		{
			DungeonCrawlerController.freezePlayerInput = true; // Freeze player
						
			float lerpStartTime;
			float lerpElapsedTime;
			float lerpComplete;

			lerpStartTime = Time.time;
			lerpComplete = 0f;

			Vector3 currentRot = Camera.transform.rotation.eulerAngles;
			Vector3 newRot = Camera.transform.rotation.eulerAngles + new Vector3(3f, 0, 0);

			while (lerpComplete < 1f)
			{
				lerpElapsedTime = Time.time - lerpStartTime;
				lerpComplete = lerpElapsedTime / duration;

				Camera.transform.eulerAngles = Vector3.Lerp(currentRot, newRot, lerpComplete);

				yield return null;
			}

			lerpStartTime = Time.time;
			lerpComplete = 0f;

			currentRot = Camera.transform.rotation.eulerAngles;
			newRot = Camera.transform.rotation.eulerAngles + new Vector3(-3f, 0, 0);

			while (lerpComplete < 1f)
			{
				lerpElapsedTime = Time.time - lerpStartTime;
				lerpComplete = lerpElapsedTime / duration;

				Camera.transform.eulerAngles = Vector3.Lerp(currentRot, newRot, lerpComplete);

				yield return null;
			}

			DungeonCrawlerController.freezePlayerInput = false;
		}
	}
}