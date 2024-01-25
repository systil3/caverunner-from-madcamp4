using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartEndManager : MonoBehaviour
{
    public GameObject gamestartpanel;
    public GameObject gameoverpanel;
    public TextMeshProUGUI timetext;
    public TextMeshProUGUI stagetext;
    // Start is called before the first frame update
    void Start()
    {
        gameoverpanel.SetActive(false);
        bool isDead = PlayerPrefs.GetInt("isdead") == 1;
        float gametime = PlayerPrefs.GetFloat("elapsedtime");
        int gameStage = PlayerPrefs.GetInt("stage");
        if(isDead){
            gameoverpanel.SetActive(true);
            gamestartpanel.SetActive(false);
            int minutes = Mathf.FloorToInt(gametime / 60);
            int seconds = Mathf.FloorToInt(gametime % 60);
            float milliseconds = (gametime * 100) % 100;

        // 시간을 "00:00:00.00" 형태의 문자열로 형식화
            string formattedTime = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
            timetext.text = "Time Record: "+formattedTime;
            stagetext.text = "Stage: " + gameStage.ToString();
            PlayerPrefs.SetInt("isdead", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void gamestart(){
        PlayerPrefs.SetInt("isresume", 0);
        SceneManager.LoadScene("SampleScene");
        Cursor.visible = false;
    }
    public void reumegame(){
        Cursor.visible = false;
        PlayerPrefs.SetInt("isresume", 1);
        SceneManager.LoadScene("SampleScene");
    }

}