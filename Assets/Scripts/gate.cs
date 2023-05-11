using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gate : MonoBehaviour
{

    //holds scene to switch to
    public string mapName; 

    public Text timeText, killsText, rankText, scoreText;
    private float secondsCount = 0f;
     private int minuteCount = 0;


     private bool[] openGates = {false, false, false, false};

     [Header("Audio")]
    AudioSource soundFX;
    public AudioClip gateSFX;
  
    //defines ammo text object
    void Start(){                 soundFX = GetComponent<AudioSource>();  }

    void Update(){timer();}
    
    //when player collides with collectiable
    private void OnTriggerEnter(Collider other)
    {
        //First gate
        if(gameObject.name == "Trigger1"){
            
            if(other.CompareTag("Player") && ControllerH.hasKeys[0] == true || ControllerH.hasKeys[2] == true){
                Debug.Log("Open");
                if(!openGates[0]){
                GameObject.Find("FirstGate").GetComponent<Animator>().Play("open1");
                soundFX.PlayOneShot(gateSFX, 2);
                }
                openGates[0] = true;
            }
            else if (other.CompareTag("Player"))
            {
                GameObject.Find("Messages").transform.Find( "key" ).gameObject.SetActive(true);
                Invoke("disableMsg", 2f);
            }
        }
        //Second gate
        else if (gameObject.name == "Trigger2"){
            if(other.CompareTag("Player") && ControllerH.hasKeys[2] == true){
                Debug.Log("Open");
                if(!openGates[1]){
                GameObject.Find("SecondGate").GetComponent<Animator>().Play("open1");
                soundFX.PlayOneShot(gateSFX, 2);
                }
                openGates[1] = true;
            }
            else if (other.CompareTag("Player"))
            {
                GameObject.Find("Messages").transform.Find( "key" ).gameObject.SetActive(true);
                Invoke("disableMsg", 2f);
            }
        }
        //Third gate
        else if (gameObject.name == "Trigger3"){
            if(other.CompareTag("Player") && ControllerH.hasKeys[2] == true){
                soundFX.PlayOneShot(gateSFX, 2);
                            
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
            else if (other.CompareTag("Player"))
            {
                GameObject.Find("Messages").transform.Find( "key" ).gameObject.SetActive(true);
                Invoke("disableMsg", 2f);
            }
        }
        /*fourth gate
        else if (gameObject.name == "Trigger4"){
            if(other.CompareTag("Player") && ControllerH.hasKeys[1] == true || ControllerH.hasKeys[2] == true){
                Debug.Log("Open");
                if(!openGates[3]){
                GameObject.Find("FourthGate").GetComponent<Animator>().Play("open1");
                soundFX.PlayOneShot(gateSFX, 2);
                }
                openGates[3] = true;
            }
            else if (other.CompareTag("Player"))
            {
                GameObject.Find("Messages").transform.Find( "key" ).gameObject.SetActive(true);
                Invoke("disableMsg", 2f);
            }
        }*/
        //fourth gate
        else if (gameObject.name == "Trigger4"){
            
            if (other.CompareTag("Player")  && ControllerH.hasKeys[1] == true || ControllerH.hasKeys[2] == true){
                soundFX.PlayOneShot(gateSFX, 2);
                            
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
            else if (other.CompareTag("Player"))
            {
                GameObject.Find("Messages").transform.Find( "key" ).gameObject.SetActive(true);
                Invoke("disableMsg", 2f);
            }
        }

    }


    public void disableMsg(){
        GameObject.Find("Messages").transform.Find( "key" ).gameObject.SetActive(false);
        GameObject.Find("Messages").transform.Find( "secret" ).gameObject.SetActive(false);

    }


     public void timer(){
         //set timer UI
         secondsCount += Time.deltaTime;
         
         if(secondsCount >= 60){
             minuteCount++;
             secondsCount = 0;
         } 
     }
}
