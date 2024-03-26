using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AnimatedAvatar : ScriptableObject
{
    public Sprite[] spritesIdle;
    public Sprite[] talkSprites;
    public Sprite[] tiredSprites;
    public bool highLowHarmonics;
}
