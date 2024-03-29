//==============================================================
// Enemy AI for dungeon master like games  (C) 2024 Zombisoft
// Attach to the enemy
//==============================================================

using System.Collections;
using UnityEngine;

namespace zombisoft
{
	public class EnemyAI : MonoBehaviour
	{
		//==============================================================
		// Variables
		//==============================================================
		private Transform Player; // The player transform. The one to chase.. :)

		[Header("Enemy movement")]
		public float castDistance = 4f; // Physics RayCast distance to check obsticle
		public float moveDuration = 0.5f; // Move duration in seconds
		public float rotateDuration = 0.3f; // Rotate duration in seconds
		public float delayBetweenMove = 0.5f; // Delay between moving the enemy
		public int moveDistance = 4; // Unity units to move
		public GameObject tempCollider; // Temporary collider to prevent player & enemy move to the same position

		[Header("Enemy distances")]
		public float awakeDistance = 17f; // Enemy awake distance (Wake up and chase)
		public float sleepDistance = 21f; // Enemy sleep distance (Go back to sleep)

		// Attacks not implemented in Demo Scene
		[Header("Enemy attack")]
		public float maxRangeAttackDistance = 17f; // Maximum distance when using range attacks
		public float minRangeAttackDistance = 7f; // Minimum distance when using range attacks
		public float areaAttackDistance = 5f; // Maximum distance when using area attacks
		public float delayBetweenAreaAttack = 3f; // Pause between attacking
		public float delayBetweenRangeAttack = 5f; // Pause between attacking

		private bool canMoveFwd; // Enemy can move forward
		private bool canMoveBack; // Enemy can move backward
		private bool canMoveLeft; // Enemy can move left
		private bool canMoveRight; // Enemy can move right

		//==============================================================
		// Enemy States
		//==============================================================
		private bool enemyAwake; // Enemy is awaken
		private bool enemyChase; // Enemy is chasing the player
		private bool rangeAttack; // Enemy is in range-attack distance
		private bool areaAttack; // Enemy is in area-attack distance

		//==============================================================
		// Start
		//==============================================================	
		void Start()
		{
			Player = GameObject.FindWithTag("Player").transform; // Cache the player transform
		}

		//==============================================================
		// Update
		//==============================================================
		void Update()
		{
			//==============================================================
			// Get the distance to the player
			//==============================================================
			float distance = Vector3.Distance(transform.position, Player.position);

			//==============================================================
			// Wake up Enemy
			//==============================================================
			if (distance <= sleepDistance && !enemyAwake) // Wake up
			{
				enemyAwake = true;

				//==============================================================
				// You can put stuff here.
				// Let the player know that there is an enemy near. Sound etc.. 
				//==============================================================

				StartCoroutine("Chase", false); // Initially, just rotate the enemy towards the player
			}
			else if (distance > sleepDistance) // Go back to sleep
			{
				enemyChase = false;
				enemyAwake = false;
			}

			//==============================================================
			// Chase the player
			//==============================================================
			if (distance <= awakeDistance && distance > areaAttackDistance) // Within chase range
			{
				if (!enemyChase)
					StartCoroutine("Chase", true); // Begin to chase the player. Rotate and move
			}

			if (distance <= awakeDistance && distance < areaAttackDistance) // Within area attack range
			{
				if (!enemyChase)
					StartCoroutine("Chase", false); // Enemy in area attack position. Just rotate
			}

			//==============================================================
			// Execute Area Attack
			//==============================================================
			if (distance <= areaAttackDistance)
			{
				if (!areaAttack)
					StartCoroutine("EnemyAreaAttack");				
			}

			//==============================================================
			// Execute Range Attack
			//==============================================================
			if (distance <= maxRangeAttackDistance && distance >= minRangeAttackDistance)
			{
				if (!rangeAttack)
					StartCoroutine("EnemyRangeAttack");	
			}
		}

		//==============================================================
		// Coroutine: Chase the player
		//==============================================================
		IEnumerator Chase(bool move)
		{
			enemyChase = true;

			float dot = Vector3.Dot(transform.forward, (Player.position - transform.position).normalized); // Check if enemy is facing player

			if (dot < 0.7f) // If not facing player, Rotate the enemy
			{
				Vector3 currentRot = transform.rotation.eulerAngles; // Enemy current rotation
				Vector3 newRot = Vector3.zero; // Enemy new rotation

				float deltaX = Mathf.Abs(Player.transform.position.x - transform.position.x);
				float deltaZ = Mathf.Abs(Player.transform.position.z - transform.position.z);

				// If the distance to the player in the x-axis is greater than in the z-axis
				if (deltaX > deltaZ && Player.transform.position.x > transform.position.x)
					newRot.y = 90f;
				else if (deltaX > deltaZ && Player.transform.position.x < transform.position.x)
					newRot.y = -90f;

				// If the distance to the player in the z-axis is greater than in the x-axis
				if (deltaZ > deltaX && Player.transform.position.z > transform.position.z)
					newRot.y = 0f;
				else if (deltaZ > deltaX && Player.transform.position.z < transform.position.z)
					newRot.y = 180f;

				// Lerp rotate enemy
				float lerpStartTime;
				float lerpElapsedTime;
				float lerpComplete;

				lerpStartTime = Time.time;
				lerpComplete = 0f;

				while (lerpComplete < 1.0f)
				{
					lerpElapsedTime = Time.time - lerpStartTime;
					lerpComplete = lerpElapsedTime / rotateDuration;
					float angle = Mathf.LerpAngle(currentRot.y, newRot.y, lerpComplete);
					transform.eulerAngles = new Vector3(0, angle, 0);
					yield return null;
				}
			}

			if (move)
				yield return StartCoroutine("MoveEnemy", calcEnemyPos()); // Get new pos and move enemy
			else
			{
				enemyChase = false;
				yield return null;
			}
		}

		//==============================================================
		// Coroutine: MoveEnemy
		//==============================================================
		IEnumerator MoveEnemy(Vector3 endPos)
		{
			Vector3 curPos = transform.position; // Get current position

			// Prevent Enemies and player to move to the same position
			GameObject tmpCollider = Instantiate(tempCollider, endPos, transform.rotation); // Create protecting collider
	
			// Lerp move enemy
			float lerpStartTime;
			float lerpElapsedTime;
			float lerpComplete;

			lerpStartTime = Time.time;
			lerpComplete = 0f;

			while (lerpComplete < 1.0f)
			{
				lerpElapsedTime = Time.time - lerpStartTime;
				lerpComplete = lerpElapsedTime / moveDuration;
				transform.position = Vector3.Lerp(curPos, endPos, lerpComplete);
				yield return null;
			}

			Destroy(tmpCollider); // Destroy protecting collider

			yield return new WaitForSeconds(Random.Range(delayBetweenMove, delayBetweenMove * 2f)); // Short delay before move again

			enemyChase = false;
			yield return null;
		}

		//==============================================================
		// Coroutine Area Attack
		//==============================================================
		IEnumerator EnemyAreaAttack()
		{
			areaAttack = true;

			yield return new WaitForSeconds(Random.Range(delayBetweenAreaAttack, delayBetweenAreaAttack + 2f)); // Delay between area attacks

			//==============================================================
			// Cancel attack if tha player has move away
			//==============================================================
			float distance = Vector3.Distance(transform.position, Player.position);
			if (distance > areaAttackDistance)
			{
				areaAttack = false; // Ready for new Attack
				yield break;
			}

			//==============================================================
			// Put Area Attack Here!!!
			//==============================================================

			print("Area attack!");

			//==============================================================
			// Put Area Attack Here!!!
			//==============================================================

			areaAttack = false; // Ready for new Attack
			yield return null;
		}

		//==============================================================
		// Coroutine Enemy Range Attack
		//==============================================================
		IEnumerator EnemyRangeAttack()
		{
			rangeAttack = true;

			//==============================================================
			// Enemy Range Attack
			//==============================================================
			RaycastHit hit;

			if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, maxRangeAttackDistance)) // Does the ray intersect the player
			{
				if (hit.transform.tag == "Player")
				{
					Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);

					//==============================================================
					// Put Range Attack Here!!!
					//==============================================================

					print("Range attack!");

					//==============================================================
					// Put Range Attack Here!!!
					//==============================================================

					yield return new WaitForSeconds(Random.Range(delayBetweenRangeAttack, delayBetweenRangeAttack + 2f)); // Delay between range attacks
				}
			}
			else
				Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.green);

			rangeAttack = false; // Ready for new Attack
			yield return null;
		}

		//==============================================================
		// Calculate the new enemy position
		//==============================================================
		public Vector3 calcEnemyPos()
		{
			RayCastCheck(); // Check obsticle in all four directions

			Vector3 newEnemyPos; // New enemy position

			if (canMoveFwd) // Enemy can move forward
				newEnemyPos = transform.position + transform.forward * moveDistance;

			else if (canMoveLeft && canMoveRight) // Enemy can move both left and right
			{
				Vector3 enemyPosRight = transform.position + transform.right * moveDistance;
				Vector3 enemyPosleft = transform.position - transform.right * moveDistance;

				if (Vector3.Distance(enemyPosRight, Player.position) > Vector3.Distance(enemyPosleft, Player.position)) // Which destination is closest to player and choose that path
					newEnemyPos = transform.position - transform.right * moveDistance; // Move left
				else
					newEnemyPos = transform.position + transform.right * moveDistance; // Move right
			}

			else if (canMoveLeft)// Enemy can move left
				newEnemyPos = transform.position - transform.right * moveDistance;

			else if (canMoveRight)// Enemy can move right
				newEnemyPos = transform.position + transform.right * moveDistance;

			else if (canMoveBack)// Enemy can move back
				newEnemyPos = transform.position - transform.forward * moveDistance;

			else // Enemy can't move in any direction
				newEnemyPos = transform.position; // Stand still :)

			return newEnemyPos;
		}

		//==============================================================
		// RayCast Check.
		//==============================================================
		void RayCastCheck()
		{
			if (CheckObsticle(castDistance, transform.TransformDirection(Vector3.forward)))
				canMoveFwd = false;
			else
				canMoveFwd = true;

			if (CheckObsticle(castDistance, transform.TransformDirection(Vector3.left)))
				canMoveLeft = false;
			else
				canMoveLeft = true;

			if (CheckObsticle(castDistance, transform.TransformDirection(Vector3.right)))
				canMoveRight = false;
			else
				canMoveRight = true;

			if (CheckObsticle(castDistance, transform.TransformDirection(Vector3.back)))
				canMoveBack = false;
			else
				canMoveBack = true;
		}

		//==============================================================
		// Check Obsticle
		//==============================================================
		bool CheckObsticle(float Distance, Vector3 endPos)
		{
			RaycastHit hit;

			if (Physics.Raycast(transform.position, endPos, out hit, Distance)) // Does the ray intersect any objects
			{
				if ((hit.transform.tag == "Obsticle" || hit.transform.tag == "Enemy" || hit.transform.tag == "TmpCollider")) // Is it an obsticle..
				{
					Debug.DrawRay(transform.position, endPos * hit.distance, Color.red); // Debug
					return true;
				}
				else
					return false;
			}
			else
			{
				Debug.DrawRay(transform.position, endPos * 1000, Color.green); // Debug
				return false;
			}
		}

		void OnDrawGizmosSelected() // Debug
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, awakeDistance);
		}
	}
}