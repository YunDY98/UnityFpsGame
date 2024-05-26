using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;
using TMPro;
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
 
    public int rotSpeed = 2000;
    private int maxRotSpeed = 5000;

    // 게임 상태 ui 변수

    public GameObject gameLabel;

    // esc
    public GameObject esc;
    public GameObject option;

    public GameObject skill;

    public Slider rotSpeedSlider;

  
    public TextMeshProUGUI rotSpeedText;

    public Text gameText;

    
    public PlayerMove player;

   
   

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
        
       
        //gState = GameState.Ready;
        gState = GameState.Run;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
       // gameText = gameLabel.GetComponent<Text>();

      //  gameText.text = "Ready .. ";
        
      //  gameText.color = new Color32(255,185,0,255);
        // 게임 준비 -> 게임 중 상태로 전환 
       // StartCoroutine(ReadyToStart());

        //player = GameObject.Find("Player").GetComponent<PlayerMove>();
        
        
    }

    // IEnumerator ReadyToStart()
    // {
    //     yield return new WaitForSeconds(0.1f);

    //     gameText.text = "Go!";
        
    //     yield return new WaitForSeconds(.5f);

    //     gameLabel.SetActive(false);

    //     gState = GameState.Run;
    // }

    // Update is called once per frame
    void Update()
    {
    //     if(gState != GameState.Run)
    //         return;
       
        if(player.hp <= 0)
        {
            player.GetComponentInChildren<Animator>().SetFloat("MoveMotion",0f);

            gameLabel.SetActive(true);

            gameText.text = "Game Over";

            gameText.color = new Color32(255,0,0,255);

            gState = GameState.GameOver;
                
        }

        ESC();
        Skill();

        
    }

    public void Option()
    {
        option.SetActive(!option.activeSelf);

        if(option.activeSelf)
        {
            
            if(PlayerPrefs.HasKey("rotSpeed"))
            {
                rotSpeedSlider.value = PlayerPrefs.GetInt("rotSpeed");
            
            }
            else
            {
                rotSpeedSlider.value = rotSpeed;
            }

            
            rotSpeedText.text = rotSpeed.ToString();

            
          
            
        }
        else
        {
            PlayerPrefs.Save();
            
        }
        
       
        
    }
    public void MouseSensitivity()
    {
        rotSpeed = (int)rotSpeedSlider.value;
       
        //rotSpeedText.text = $"{rotSpeed}";
        rotSpeedText.text = rotSpeed.ToString();

        PlayerPrefs.SetInt("rotSpeed", rotSpeed);
        
        
        
        
    }

    public void Skill()
    {
        if(esc.activeSelf == true)
        {
            return;
        }
        
        
        if(Input.GetKeyDown(KeyCode.K))
        {
            // 스킬레벨 정보 업데이트 
           // PlayerStats.playerStats.UpdateSkillText();

            ToggleCursor();
            skill.SetActive(!skill.activeSelf);
           
        }

    }

    public void ESC()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursor();
            esc.SetActive(!esc.activeSelf);
            Time.timeScale = isCursorVisible ? 0 : 1;
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
