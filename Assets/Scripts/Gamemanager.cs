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

    
    private static GameManager _instance;
    // Start is called before the first frame update
    
    public static GameManager gameManager
    {
        get
        {

            return _instance;
        }
        set
        {

        }
    }
 
    public float rotSpeed = 2000f;

    // 게임 상태 ui 변수

    public GameObject gameLabel;

    public GameObject esc;

    public Text gameText;
    
    
    
    public PlayerMove player;

    public float RotSpeed;

    //움직임 관련 일시정지
    public bool isMove = true;

    //캐릭터 정면으로 보기 
    public bool showFace = false;

    private bool isCursorVisible;
   
    
  
    //현재 게임 상태 변수
    public GameState gState;

    private void Awake()
    {
       
         // 이미 인스턴스가 존재한다면 파괴합니다.
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }



    void Start()
    {   

        gState = GameState.Ready;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursor();
            esc.SetActive(!esc.activeSelf);
        }
    }


    public void RestartGame()
    {
        Time.timeScale = 0;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        SceneManager.LoadScene(1);
    }

    void ToggleCursor()
    {
        isCursorVisible = !isCursorVisible;
        Cursor.visible = isCursorVisible;
        Cursor.lockState = isCursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    

   

}
