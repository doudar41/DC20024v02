using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lockedDoor : MonoBehaviour
{
    public string keyEvent; //This event being true will unlock the door.
    public string lockedDoorEvent; //This event being true will cause the door to ALWAYS be unlocked when the player retruns to it.
    public bool locked = true;
    public GameObject lockedCollider; //collider that checks to see if the door can be opened.
    public GameObject obsticleCollider; //collider that denies entry;
    public GameObject lockSymbol;
    public GameObject unlockedCollider; //collider that opens the door

    private void Update()
    {
        if(gameManager.gm.levelConcierge.checkBoolByString(lockedDoorEvent))
        {
            locked = false;
        }

        if (locked != true)
        {
            lockSymbol.SetActive(false);
            obsticleCollider.SetActive(false);
            lockedCollider.SetActive(false);
            unlockedCollider.SetActive(true);
        } else
        {
            lockSymbol.SetActive(true);
            obsticleCollider.SetActive(true);
            lockedCollider.SetActive(true);
            unlockedCollider.SetActive(false);
        }

    }
}
