//==============================================================
// Screen Fader
//==============================================================

using UnityEngine;

namespace zombisoft
{
	public class Fader : MonoBehaviour
	{
		private Texture uitexture;
		private Color uicolor;
		private float duration;
		private int fadeDir = 1;
		private float alpha;
		private float TimeDelta;
		private bool Started;
		private static bool StartFadeIn;
		private static Fader instance;

		void Awake()
		{
			instance = this;
			if (StartFadeIn)
			{
				alpha = 0.001f;
			}
			else
			{
				alpha = 0.999f;
			}
			DontDestroyOnLoad(gameObject);
		}

		public static void FadeOut(Color color, float duration)
		{
			if (instance == null)
			{
				StartFadeIn = false;
				CreateFader();
			}
			FadeInOut(-1, color, duration);
		}

		public static void FadeIn(Color color, float duration)
		{
			if (instance == null)
			{
				StartFadeIn = true;
				CreateFader();
			}
			FadeInOut(1, color, duration);
		}

		private static void FadeInOut(int Fade, Color color, float duration)
		{
			instance.uicolor = color;
			instance.duration = duration;
			instance.fadeDir = Fade;
			instance.uitexture = CreateTexture();
			instance.Started = true;
		}

		void OnGUI()
		{
			if (Started)
			{
				alpha += 0.5f / duration * Time.deltaTime * fadeDir;
				alpha = Mathf.Clamp01(alpha);

				if (alpha >= 1 || alpha <= 0)
				{
					Started = false;
				}
			}

			if (alpha < 1 || alpha >= 0)
			{
				uicolor.a = alpha;
				GUI.color = uicolor;
				GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), uitexture);
			}
		}

		private static void CreateFader()
		{
			GameObject go = new GameObject("Fader");
			go.AddComponent<Fader>();
		}

		private static Texture2D CreateTexture()
		{
			Texture2D newfade = new Texture2D(1, 1);
			newfade.SetPixel(0, 0, Color.white);
			newfade.SetPixel(1, 0, Color.white);
			newfade.SetPixel(0, 1, Color.white);
			newfade.SetPixel(1, 1, Color.white);
			newfade.Apply();
			return newfade;
		}
	}
}
