using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level1Concierge : MonoBehaviour
{
    public bool[] bools;
    public string[] eventName;

    private void Start()
    {
    }

    public void switchBool(string eventString, bool setting)
    {
        if (getBool(eventString) == -1)
        {
            throw new System.Exception("Not a listed Bool!!");
        }

        bools[getBool(eventString)] = setting;
    }

    public bool checkBoolByString(string eventString)
    {
        if(getBool(eventString) == -1)
        {
            throw new System.Exception("Not a listed Bool!!");
        }

        return bools[getBool(eventString)];
    }

    int getBool(string eventString)
    {
        for (int i = 0; i < eventName.Length; i++)
        {
            if (eventString == eventName[i])
            {
                return i;
            }
        }
        return -1;
    }
}
