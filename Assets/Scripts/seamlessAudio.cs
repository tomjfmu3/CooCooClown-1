using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seamlessAudio : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GameObject[] music = GameObject.FindGameObjectsWithTag("music");

        if(music.Length > 1) { Destroy(this.gameObject); }
        DontDestroyOnLoad(this.gameObject);


    }

}
