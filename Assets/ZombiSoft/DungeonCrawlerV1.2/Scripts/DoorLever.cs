//==============================================================
// DoorLever (C) 2024 Zombisoft
// Attach to door
//==============================================================

using System.Collections;
using UnityEngine;

namespace zombisoft
{
	public class DoorLever : MonoBehaviour
	{
		//==============================================================
		// Variables DOORS
		//==============================================================

		[Header("Connect the Animator")]
		public Animator leverAnimator1; // Lever Animator 1
		public Animator leverAnimator2; // Lever Animator 2

		public Collider leverCollider1; // Lever1 
		public Collider leverCollider2; // Lever2 

		public Transform doorTransform; // Door transform
		public Collider DoorCollider; // Door Collider

		public float delayBeforeOpen = 0.5f;
		public float distanceToMove = 3.45f; // How far will it go
		public float timeDuringOpening = 0.5f;
		public float timeDuringClosing = 0.3f;

		// Distance to Lever
		public float DistanceToButton = 4.0f;

		// Main Camera
		private Camera cam;

		// Door closed and opened positions
		private Vector3 closedPosition;
		private Vector3 openedPosition;

		// LERP Variables
		private float timeStartLerp;
		private float timeSinceStarted;
		private float doorOpeningComplete;
		private float doorClosingComplete;

		// Logic
		private bool doorMoving = false;
		private bool doorOpen = false;

		//==============================================================
		// Start
		//==============================================================
		void Start()
		{
			// Define closed & opened positions
			closedPosition = doorTransform.transform.position;
			openedPosition = doorTransform.transform.position + doorTransform.TransformDirection(Vector3.up) * distanceToMove;

			cam = Camera.main;
		}

		//==============================================================
		// Update
		//==============================================================

		void Update()
		{
			if (Input.GetMouseButtonDown(0)) // if left mouse button pressed
			{
				Ray ray = cam.ScreenPointToRay(Input.mousePosition);

				RaycastHit hit;

				// Raycast on lever1 colliders
				if (leverCollider1.Raycast(ray, out hit, DistanceToButton))
				{
					leverAnimator1.Play("Lever");

					if (!doorMoving)
					{
						if (!doorOpen)
							StartCoroutine("OpenDoor");
						else
							StartCoroutine("CloseDoor");
					}
				}

				// Raycast on lever2 colliders
				else if (leverCollider2.Raycast(ray, out hit, DistanceToButton))
				{
					leverAnimator2.Play("Lever");

					if (!doorMoving)
					{
						if (!doorOpen)
							StartCoroutine("OpenDoor");
						else
							StartCoroutine("CloseDoor");
					}
				}
			}
		}

		//==============================================================
		// Open the door
		//==============================================================
		IEnumerator OpenDoor()
		{
			doorMoving = true;

			yield return new WaitForSeconds(delayBeforeOpen);

			timeStartLerp = Time.time;
			doorOpeningComplete = 0f;

			while (doorOpeningComplete < 1.0f)
			{
				timeSinceStarted = Time.time - timeStartLerp;
				doorOpeningComplete = timeSinceStarted / timeDuringOpening;

				doorTransform.transform.position = Vector3.Lerp(closedPosition, openedPosition, doorOpeningComplete);

				yield return null;
			}

			DoorCollider.gameObject.SetActive(false);

			doorMoving = false;
			doorOpen = true;
		}

		//==============================================================
		// Close the door
		//==============================================================
		IEnumerator CloseDoor()
		{
			DoorCollider.gameObject.SetActive(true);

			doorMoving = true;

			yield return new WaitForSeconds(delayBeforeOpen);

			timeStartLerp = Time.time;
			doorClosingComplete = 0f;

			while (doorClosingComplete < 1.0f)
			{
				timeSinceStarted = Time.time - timeStartLerp;
				doorClosingComplete = timeSinceStarted / timeDuringClosing;

				doorTransform.transform.position = Vector3.Lerp(openedPosition, closedPosition, doorClosingComplete);

				yield return null;
			}

			doorMoving = false;
			doorOpen = false;
		}
	}
}