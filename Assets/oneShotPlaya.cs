using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oneShotPlaya : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    public void playOneShot(string eventPath)
    {
        FMODUnity.RuntimeManager.PlayOneShot(eventPath);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
