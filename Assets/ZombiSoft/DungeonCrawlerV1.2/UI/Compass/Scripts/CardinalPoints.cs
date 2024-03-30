//==============================================================
// DungeonCrawlerController - CardinalPoints
// Attach to CardinalPoints (Compass)
//==============================================================

using UnityEngine;
using UnityEngine.UI;

namespace zombisoft
{
	public class CardinalPoints : MonoBehaviour
	{

		public static CardinalPoints Instance;
		public RawImage compass;

		//==============================================================
		// Awake
		//==============================================================
		private void Awake()
		{
			if (CardinalPoints.Instance != null)
			{
				Destroy(gameObject);
				return;
			}
			Instance = this;
		}

		//==============================================================
		// Compass new direction.
		//==============================================================
		public void NewDirection(Transform player)
		{
			compass.uvRect = new Rect(player.localEulerAngles.y / 360f, 0, 1, 1);
		}
	}
}