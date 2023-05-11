using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class WallDropper : MonoBehaviour
{
	 [Header("Audio")]
    AudioSource soundFX;
	public AudioClip gateSFX;
	public bool dropped = false;
	public bool droppedMain = false;

    //used for showing end menu

    public Text timeText, killsText, rankText, scoreText;
private float secondsCount = 0f;
     private int minuteCount = 0;
	

    // Start is called before the first frame update
    void Start()
    {
         soundFX = GetComponent<AudioSource>(); 
    }

    // Update is called once per frame
    void Update()
    {
                timer();

    }
	
	  private void OnTriggerEnter(Collider other)
    {
		if(!dropped){
			if(other.CompareTag("Player")){
				dropped=true;
				
                Debug.Log("Open");
                
                GameObject.Find("SpawnWall").GetComponent<Animator>().Play("dropWall1");
                soundFX.PlayOneShot(gateSFX, 2);
                
                
                
            }
		}
		if(gameObject.name == "TentTrigger"){
            
            if(other.CompareTag("Player") && ControllerH.hasKeys[0] == true || ControllerH.hasKeys[2] == true){
                Debug.Log("Open");
                if(!droppedMain){
                GameObject.Find("TentDoor").GetComponent<Animator>().Play("openEndWall");
                                Invoke("transition", 5f);

                
                }
                droppedMain = true;
            }
            else if (other.CompareTag("Player"))
            {
                GameObject.Find("Messages").transform.Find( "key" ).gameObject.SetActive(true);
                Invoke("disableMsg", 2f);
            }
        }
	}

    public void timer(){
         //set timer UI
         secondsCount += Time.deltaTime;
         
         if(secondsCount >= 60){
             minuteCount++;
             secondsCount = 0;
         } 
     }

     public void transition(){
//soundFX.PlayOneShot(gateSFX, 2);

                GameObject.Find("Canvas").transform.Find( "Ending" ).gameObject.SetActive(true);
                    killsText.text = "Kills: " + ControllerH.killCount;
                    timeText.text = "Time: " + minuteCount +"m:"+(int)secondsCount + "s";
                    scoreText.text = "Score: " + ControllerH.score;

                    //RANK CALCULATIONS
                    int rankScore = 0;
                    rankScore += ControllerH.score;
                    rankScore += ControllerH.killCount * 10;
                    rankScore -= (int)secondsCount + (minuteCount * 60);
                    Debug.Log(rankScore);
                    if(rankScore >= 400){rankText.text = "Rank: " + "S";}
                    else if(rankScore >= 300){rankText.text = "Rank: " + "A";}
                    else if(rankScore >= 200){rankText.text = "Rank: " + "B";}
                    else if(rankScore >= 100){rankText.text = "Rank: " + "C";}
                    else if(rankScore < 100){rankText.text = "Rank: " + "D";}
                    GameObject.Find("Player").GetComponent<MouseLooker>().enabled = false;
                    GameObject.Find("Main Camera").GetComponent<ShooterV2>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
     }
	
}
