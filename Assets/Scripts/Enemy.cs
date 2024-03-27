using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class Enemy : MonoBehaviour
{

    public int Constitution = 5, Agility = 5, Intellect = 5, Luck = 5;
    public float health, stamina;
    [Range(0, 8)]
    public int runeNumber = 5;
    public  bool[] runeSongSequence = new bool[16];
    public RuneSong enemyRuneSong;
    [SerializeField] int runeStrenght = 10;

    [SerializeField] RuneEffect[] runeEffect;
    [SerializeField] bool randomSequence = false;

    private bool doesWait = true;
    private int countPassed;
    private int waitForActionStart;
    [SerializeField] private Image icon;
    [SerializeField] private Slider progressBar;
    private int countDownToActionEnd;
    [SerializeField] private int howLongIsTurn;

    public UnityEvent<Enemy> ToPlayer;
    private bool action =false;
    private int countInSequence =0;
    public  UnityEvent onSequenceTrue;

    public Hero hero;
    int randomInt = 0;
    bool actionEnds = true;

    public TextMeshProUGUI healthText;

    void Start()
    {
        waitForActionStart = howLongIsTurn - Agility;
        countDownToActionEnd = howLongIsTurn;
        healthText.text = health.ToString();

        if (randomSequence) GeneratingSong(runeNumber);
        else
        {
            Runes runes = new Runes();
            enemyRuneSong = runes.ShowSequence(runeSongSequence);
            enemyRuneSong.strenghtAmount = runeStrenght;
            enemyRuneSong.runeEffect = runeEffect[0];
        }
    }

    void FillArrayWithConstant(int constant)
    {

    }
    void GeneratingSong(int number)
    /*
    @todo in progress writing
    @method GeneratingSong

    @param { int } number 
        the number of runes generated with a value of 0 to 15
    */
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

        for(int i = 0; i < runeSongSequence.Length; i++)
        {
            if (tempList.Contains(i))
            {
                runeSongSequence[i] = true;
            }
            else
            {
                runeSongSequence[i] = false;
            }
        }
        Runes runes = new Runes();
        enemyRuneSong = runes.ShowSequence(runeSongSequence);
        enemyRuneSong.strenghtAmount = runeStrenght;
        enemyRuneSong.runeEffect = runeEffect[0];
    }
    public void GetClick()
    /*
    @todo inProgress: confirming
    @method GetClick
        gets the current metronome click
        gets the current click value for use as comparison for event triggers
        indicates the UI feedback for the player
    */
    {
        //print((float)countPassed / (float)waitForActionStart);
        if (doesWait)
        {
            countPassed++;
            progressBar.value = (float)countPassed / (float)waitForActionStart;
            if (countPassed >= waitForActionStart)
            {
                icon.color = Color.green;
                doesWait = false;
                countPassed = 0;
                if (!action) { randomInt = Random.Range(0, 10); }
                
            }
        }
        else
        {
            if (randomInt == countPassed && !action) StartActionSequence();
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

    public void SendThisEnemyToPlayer()
    /*
    @method SendThisEnemyToPlayer
        passes the invoking enemy ToPlayer() function for the player to use the invoker
    */
    {
        ToPlayer.Invoke(this);
    }

    public void HarmEnemy(float amount)
    /*
    @todo inProgress confirming
    @method HarmEnemy
        deals harm to another enemy
    @param { float } amount
        the amount of damage dealt to the enemy
    */
    {
        print(this.name+" enemy damaged");
        health -= amount;
        healthText.text = health.ToString();
    }

    public void StartActionSequence()
    /*
    @todo inProgress confirming
    @method StartActionSequence
        starts the action sequence for the enemy
    */
    {
        if (!doesWait && !action)
        {
            action = true;
            icon.color = Color.red;
            doesWait = true;
            countPassed = 0;
        }
    }

    public void LaunchSequence()
    /*
    @method LaunchSequence()
        starts a sequence for this enemy's runeSongSequence
    */
    {
        if (countInSequence == runeSongSequence.Length)
        {
            action = false; countInSequence = 0; 
        }

        if (!action) return;
        if (runeSongSequence[countInSequence]) { onSequenceTrue.Invoke(); }
        countInSequence++;
    }

    public void HarmPlayer()
    /*
    @todo inProgress
    @method HarmPlayer
        deals harm to the player
    */
    {
        //print(this.name + " harm player");

        hero.GetDamage(1);
    }

}
