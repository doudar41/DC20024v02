using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lockedCollider : MonoBehaviour
{
    public string keyEvent; //This event being true will unlock the door.
    public string lockedDoorEvent; //This event being true will cause the door to ALWAYS be unlocked when the player retruns to it.

    protected void OnTriggerEnter(Collider t)
    {
        if (gameManager.gm.levelConcierge.checkBoolByString(keyEvent))
        {
            gameManager.gm.levelConcierge.switchBool(lockedDoorEvent, true);
        }

    }
}
