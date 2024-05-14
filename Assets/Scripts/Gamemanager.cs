using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    //게임 상태 상수
    public enum GameState
    {
        Ready,
        Run,
        GameOver

    }

    
    public static GameManager gm;
    // Start is called before the first frame update

    // 게임 상태 ui 변수

    public GameObject gameLabel;

    public Text gameText;
    
    public GameObject aim;
    
    public PlayerMove player;

    public float RotSpeed;
   
    
  
    //현재 게임 상태 변수
    public GameState gState;

    private void Awake()
    {
        if(gm == null)
        {
            gm = this;
        }
    }



    void Start()
    {   

        gState = GameState.Ready;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 

       // gameText = gameLabel.GetComponent<Text>();

        gameText.text = "Ready .. ";
        
        gameText.color = new Color32(255,185,0,255);
        // 게임 준비 -> 게임 중 상태로 전환 
        StartCoroutine(ReadyToStart());

        //player = GameObject.Find("Player").GetComponent<PlayerMove>();
        
        
    }

    IEnumerator ReadyToStart()
    {
        yield return new WaitForSeconds(0.1f);

        gameText.text = "Go!";
        aim.SetActive(true);
        yield return new WaitForSeconds(.5f);

        gameLabel.SetActive(false);

        gState = GameState.Run;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.hp <= 0)
        {
            player.GetComponentInChildren<Animator>().SetFloat("MoveMotion",0f);

            gameLabel.SetActive(true);

            gameText.text = "Game Over";

            gameText.color = new Color32(255,0,0,255);

            gState = GameState.GameOver;
                
        }
    }


    public void RestartGame()
    {
        Time.timeScale = 0;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        SceneManager.LoadScene(0);
    }

}
