using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpriteAnimation : MonoBehaviour
{

    public Image m_Image;
    public AnimatedAvatar m_SpriteArray;
    public float m_Speed = .1f;
    public int animationIndex = 0;

    private int m_IndexSprite;
    bool IsDone;
    public int timesToPlay = 1;

    int countTimes = 0;
    private void Start()
    {
        timesToPlay = m_SpriteArray.spritesIdle.Length;
        Func_PlayUIAnim();
    }
    public void Func_PlayUIAnim()
    {
        
        IsDone = false;
        StartCoroutine(Func_PlayAnimUI(ChangeAnimation(animationIndex)));
    }

    public void Func_StopUIAnim()
    {

        IsDone = true;
        StopCoroutine(Func_PlayAnimUI(ChangeAnimation(animationIndex)));
    }

    public Sprite[] ChangeAnimation(int index)
    {
        switch (index)
        {
            case 0:
                return m_SpriteArray.spritesIdle;
            case 1:
                return m_SpriteArray.talkSprites;
        }
        return m_SpriteArray.spritesIdle;
    }
    IEnumerator Func_PlayAnimUI(Sprite[] sprites)
    {
        sprites = ChangeAnimation(animationIndex);//Can't be less than idle animation
        yield return new WaitForSeconds(m_Speed);
        if (m_IndexSprite >= sprites.Length)
        {
            m_IndexSprite = 0;
        }
        m_Image.sprite = sprites[m_IndexSprite];
        m_IndexSprite += 1;

        if (IsDone == false)
        {
            StartCoroutine(Func_PlayAnimUI(sprites));
            countTimes++;
            if (countTimes > timesToPlay) { countTimes = 0; IsDone = true; StopCoroutine(Func_PlayAnimUI(ChangeAnimation(animationIndex))); }
        }
            
    }
}
