using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class Hero : MonoBehaviour
{

    public int Constitution = 5, Agility = 5, Intellect = 5, Luck = 5;
    float health, stamina;
    int heroLevel; 
    [Range(0, 8)]
    public int runeNumber = 5;
    public RuneSequence[] sequences;
    public RuneSong currentRuneSong;
    [SerializeField] int runeStrenght = 10;
    [SerializeField] int constantRuneStrenght = 2;
    [SerializeField] bool randomSequence = false;
    private bool doesWait = true;
    private int countPassed;
    private int waitForActionStart;
    [SerializeField] private Image icon;
    [SerializeField] private Slider progressBar;
    private int countDownToActionEnd;
    [SerializeField] private int howLongIsTurn;
    private bool action = false;
    private int countInSequence =0;
    public  UnityEvent onSequenceTrue;
    public TextMeshProUGUI healthText;
    Enemy currentEnemy;
    float runeEffectAmount = 0;


    public int[,] runeStorage = { { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                                {   1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0 },
                                {   1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0 },
                                {   1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0 },
                                {   1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 }
                                };


    void Start()
    {

        health = GetMaxHealthAmount();
        waitForActionStart = howLongIsTurn - Agility;
        countDownToActionEnd = howLongIsTurn;
        healthText.text = health.ToString();

        if (randomSequence) GeneratingSong(runeNumber);
        else
        {
            Runes runes = new Runes();
            currentRuneSong = runes.ShowSequence(sequences[0].runeSongSequence);
            currentRuneSong.strenghtAmount = runeStrenght;
            currentRuneSong.runeEffect = sequences[0].runeSongEffect;
        }
    }


    void GeneratingSong(int number)
    {
        List<int> tempList = new List<int>();

        tempList.Add(0);
        while (tempList.Count < number)
        {
            int temp = Random.Range(0, 15);
            if (!tempList.Contains(temp))
            {
                tempList.Add(temp);
            }
        }

        for (int i = 0; i < sequences[0].runeSongSequence.Length; i++)
        {
            if (tempList.Contains(i))
            {
                sequences[0].runeSongSequence[i] = true;
            }
            else
            {
                sequences[0].runeSongSequence[i] = false;
            }
        }
        Runes runes = new Runes();
        currentRuneSong = runes.ShowSequence(sequences[0].runeSongSequence);
        currentRuneSong.strenghtAmount = runeStrenght;
        currentRuneSong.runeEffect = sequences[0].runeSongEffect;
    }

    public void StartActionSequence()
    {
        if (!doesWait && !action)
        {
            action = true;
            icon.color = Color.red;
            doesWait = true;
            countPassed = 0;
        }
    }

    public void GetClick()
    {
        if (doesWait)
        {
            countPassed++;
            progressBar.value = (float)countPassed / (float)waitForActionStart;
            if (countPassed >= waitForActionStart)
            {
                icon.color = Color.green;
                doesWait = false;
                countPassed = 0;
            }
        }
        else
        {
            countPassed++;
            progressBar.value = (float)countPassed / (float)countDownToActionEnd;
            if (countPassed >= countDownToActionEnd)
            {
                icon.color = Color.red;
                doesWait = true;
                countPassed = 0;
            }
        }
    }


    public void LaunchSequence()
    {
        if (countInSequence == sequences[0].runeSongSequence.Length)
        {
            HarmCurrentEnemy(runeEffectAmount);
            //Apply rune song effect
            runeEffectAmount = 0;
            action = false; countInSequence = 0;
        }

        if (!action) return;

        if (sequences[0].runeSongSequence[countInSequence]) 
        { 
            onSequenceTrue.Invoke();
            //Calculate runesong effect
            CalculateSongEffect(countInSequence);

        }

        countInSequence++;
    }

    public void GetEnemy(Enemy enemy)
    {
        print(enemy.name);
        currentEnemy = enemy;
    }


    public void CalculateSongEffect(int index)
    {
        int level = currentRuneSong.sequence[index].level;
        int pos = currentRuneSong.sequence[index].position;
        int amount = currentRuneSong.sequence[index].GetRuneStrength();
        float baseResult = (currentRuneSong.strenghtAmount / 16)*amount;
        int intellectmodifier = runeStorage[level, pos] + 2 - (Intellect / 5);
        intellectmodifier = Mathf.Clamp(intellectmodifier, 1, 12);
        int playRuneDelta = Random.Range(1, intellectmodifier); // 12 is max division for damage 
        print("base rune damage " + baseResult);
        int luckMod = Random.Range(Luck, 20); //second number (20) is max luck number possible in game
        if (luckMod == Luck)
        {
            runeEffectAmount += (baseResult / 12) * playRuneDelta * 2;
        }
        else
        {
            runeEffectAmount += (baseResult / 12) * playRuneDelta;
        }

        runeEffectAmount += (Constitution * 0.1f);

        stamina -= runeEffectAmount * 0.5f;
        stamina = Mathf.Clamp(stamina, 0, GetMaxStaminaAmount()); 
        //Renew UI
        if (playRuneDelta == runeStorage[level, pos])
        {
            runeStorage[level, pos] += 1;

            print("rune updated" + runeStorage[level, pos]);
        }
    }


    public float GetMaxHealthAmount()
    {
        return Constitution * (10 + heroLevel - 1);
    }

    public float GetMaxStaminaAmount()
    {
        return Agility * (10 + heroLevel - 1);
    }


    public void HarmCurrentEnemy(float amount)
    {
        if(currentEnemy != null) {
            currentEnemy.HarmEnemy(amount);
        }
    }

    public void GetDamage(float amount)
    {
        health -= amount;
        healthText.text = health.ToString();
    }
}



[System.Serializable]
public class RuneSequence
{
    public bool[] runeSongSequence = new bool[16];
    public RuneEffect runeSongEffect;
}
