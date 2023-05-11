using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDropper1 : MonoBehaviour
{
	 [Header("Audio")]
    AudioSource soundFX;
	public AudioClip gateSFX;
	private bool dropped = false;
	public float TimeTillActivation=0.0f;
	public float TimeTillActivation2=0.0f;
	
	public string Wall;
	public string Anima;
	public GameObject Enemies;
	public GameObject Enemies2;
    // Start is called before the first frame update
    void Start()
    {
         soundFX = GetComponent<AudioSource>(); 
		 	StartCoroutine(WaitForAFewSeconds());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	IEnumerator WaitForAFewSeconds(){
		
		yield return new WaitForSeconds(TimeTillActivation);
		GameObject.Find(Wall).GetComponent<Animator>().Play(Anima);
		Enemies.SetActive(true);
		yield return new WaitForSeconds(TimeTillActivation2);
		Enemies2.SetActive(true);
	}
}
