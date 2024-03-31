using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class redKey : MonoBehaviour
{
    public string ItemEvent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gm.levelConcierge.checkBoolByString(ItemEvent))
        {
            gameObject.SetActive(false);
        }
    }

    private void Awake()
    {

    }

    protected void OnTriggerEnter(Collider t)
    {
        gameManager.gm.levelConcierge.switchBool(ItemEvent, true);
        gameObject.SetActive(false);
    }
}
