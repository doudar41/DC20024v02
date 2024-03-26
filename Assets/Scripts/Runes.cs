using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runes : MonoBehaviour
{

    public RuneSong ShowSequence(bool[] sequenceBool)
    {
        return RuneLengthAnalize(sequenceBool);
    }

    RuneSong RuneLengthAnalize(bool[] sequence)
    {
        Rune[] sequenceRune = new Rune[16];
        RuneSong tempSong = new RuneSong();

        int countFree = 0;
        for (int i = 0; i < sequence.Length; i++)
        {
            if (!sequence[i]) { countFree++; }
            else
            {
                Rune newRune = new Rune();
                newRune.position = i;
                sequenceRune[i] = newRune;
            }
        }

        List<Rune> temp = new List<Rune>();
        for (int i = 0; i < sequenceRune.Length; i++)
        {
            if (sequenceRune[i] != null)
            {
                temp.Add(sequenceRune[i]);
            }
        }

        for (int i = 0; i < temp.Count; i++)
        {
            if (i != temp.Count - 1)
            {
                int delta = temp[i + 1].position - temp[i].position;
                if (delta > 1)
                {
                    if (delta % 2 == 0)
                    {
                        sequenceRune[temp[i].position].level = temp[i].GetRuneStrength(delta);
                    }
                    else
                    {
                        sequenceRune[temp[i].position].level = temp[i].GetRuneStrength(delta - 1);
                        Rune addSmallestRune = new Rune();
                        addSmallestRune.level = 4;
                        addSmallestRune.position = temp[i].position + delta - 1;
                        sequenceRune[addSmallestRune.position] = addSmallestRune;
                    }
                }
                else
                {
                    sequenceRune[temp[i].position].level = 4;
                }
            }
            else
            {
                sequenceRune[temp[i].position].level = temp[i].GetRuneStrength(16 - temp[i].position); ;
            }
        }
        countFree = 0;

        foreach (Rune r in sequenceRune)
        {
            if (r != null)
                print("position - " + r.position + " level - " + r.level + " count - " + countFree);
            countFree++;
        }
        countFree = 0;
        foreach (Rune r in sequenceRune)
        {
            tempSong.sequence[countFree] = r;
            countFree++;
        }
        return tempSong;
    }
}

public class Rune
{
    public int position;
    public int level;
    public int GetRuneStrength()
    {
        switch (level)
        {
            case 0:
                return 16;
            case 1:
                return 8;
            case 2:
                return 4;
            case 3:
                return 2;
            case 4:
                return 1;
        }
        return 0;
    }

    public int GetRuneStrength(int length)
    {
        switch (length)
        {
            case 16:
                return 0;
            case 8:
                return 1;
            case 4:
                return 2;
            case 2:
                return 3;
            case 1:
                return 4;
        }
        return 0;
    }
}

public class RuneSong
{
    public Rune[] sequence = new Rune[16];
    public bool[] filledPositions = new bool[16];
    public RuneEffect runeEffect;
    public int strenghtAmount;

    public int FreeRuneSongSpace()
    {
        int freeSpaceNumber = 16;

        foreach (Rune r in sequence)
        {
            freeSpaceNumber -= r.GetRuneStrength();
        }
        return freeSpaceNumber;
    }
}

public enum RuneEffect
{
    damageHealth,
    heal,
    damageConstitution,
    restoreConstitution,
    invisibility,
    openLock,
    moveObsticle
}