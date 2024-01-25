using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public GameObject PlayerPrefab;
    public GameObject Player;
    public GameObject Camera;
    public GameObject PlayerDeathEffect;
    public Vector2 startPosition;
    public GameObject EnergyStateUI;
    public Sprite[] EnergystateSprite;
    public GameObject LifeStateUI;
    public Sprite[] LifestateSprite;
    public TextMeshProUGUI timetext;
    private float elapsedtime;
    private float currentTime = 0f;
    private int gamestage = 1;
    public GameObject[] startflagstage;

    private bool ispaused = false;
    public GameObject pausepanel;
    public GameObject endpanel;
    public TextMeshProUGUI endtimetext;
    // Start is called before the first frame update
    public AudioSource audioSource;
    private void Awake() {
        gameManager = this;
        startPosition = Player.transform.position;
        audioSource.loop = true;
        audioSource.Play();
    }

    // Update is called once per frame
    void Start(){
        if(PlayerPrefs.GetInt("isresume") ==0){
            currentTime = 0;
            PlayerPrefs.SetInt("isdead", 0);
            PlayerPrefs.SetInt("stage",1);
            gamestage = 1;
        }
        else{
            currentTime = PlayerPrefs.GetFloat("elapsedtime");
            gamestage = PlayerPrefs.GetInt("stage");
        }
        
        Player.transform.position = startflagstage[gamestage-1].transform.position + new Vector3(0.1f, 0,0);
    }
    void FixedUpdate() {
        if(!ispaused){
            currentTime +=Time.deltaTime;
        }
        elapsedtime = currentTime;
        int minutes = Mathf.FloorToInt(elapsedtime / 60);
        int seconds = Mathf.FloorToInt(elapsedtime % 60);
        float milliseconds = (elapsedtime * 100) % 100;

        // 시간을 "00:00:00.00" 형태의 문자열로 형식화
        string formattedTime = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        timetext.text = formattedTime;

    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            Cursor.visible = false;
            ispaused = !ispaused;
            pausepanel.SetActive(ispaused);
            Time.timeScale = ispaused ? 0 : 1;
        }
    }

    public void PlayerDeath(Collision2D collision) {
        
        Vector3 collisionNormal = collision.contacts[0].normal;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, collisionNormal);       
        Instantiate(PlayerDeathEffect, Player.transform.position, rotation);
        Destroy(Player);
        StartCoroutine(RestartCoroutine());
    }

    public void PlayerDeath() {
        Instantiate(PlayerDeathEffect, Player.transform.position, Quaternion.identity);
        Destroy(Player);
        StartCoroutine(RestartCoroutine());
    }

    IEnumerator RestartCoroutine() {
        yield return new WaitForSeconds(1.2f);
        //ResetPlayer();
        gotogameoverscene();
    }

    public void ResetPlayer() {
        SceneManager.LoadScene("SampleScene");
        /*GameObject newPlayer = Instantiate(PlayerPrefab, startPosition, Quaternion.identity);
        Player = newPlayer;

        newPlayer.GetComponent<Player>().gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        newPlayer.GetComponent<Player>().crosshair = GameObject.Find("crosshair").GetComponent<Crosshair>();
        CameraMove cameraMove = Camera.GetComponent<CameraMove>();
        cameraMove.Player = newPlayer;*/
    }

    public void RefreshEnergyState(int energy) {
        EnergyStateUI.GetComponent<UnityEngine.UI.Image>().sprite = EnergystateSprite[energy];
    }
    public void RefreshLifeState(int life) {
        Debug.Log($"{life}");
        LifeStateUI.GetComponent<UnityEngine.UI.Image>().sprite = LifestateSprite[life-1];
    }
    public void gotogameoverscene(){
        Cursor.visible = true;
        PlayerPrefs.SetInt("isdead", 1);
        PlayerPrefs.SetFloat("elapsedtime", elapsedtime);
        PlayerPrefs.SetInt("stage", gamestage);
        SceneManager.LoadScene("StartEndScene");
    }
    public void gameover(){
        endpanel.SetActive(true);
        endtimetext.text = "Your final score is: " + elapsedtime.ToString();
    }
    public void updatestage(){
        gamestage ++;
    }
}
