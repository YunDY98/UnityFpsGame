using System.Collections;   
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using Palmmedia.ReportGenerator.Core;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.AI;

public class PlayerStats : MonoBehaviour
{
    private static PlayerStats _instance;

    public static PlayerStats playerStats
    {
        get
        {

            return _instance;
        }
    }
    public enum SelectCharacter
    {
        
        MasaSchool,
        Solider,

    }
    public SelectCharacter selectCharacter;

    public PlayerMove playerMove;

    public GameObject aim;



    private PlayerData playerData;
    
    public int level;
    public int exp;
    public int gold;

    // public int masaAtk1Level;
    // public int masaAtk3Level;

    

    public GameObject[] characterMode;

    public Transform camPos;

    private int selectedIndex;

    
    public int sceneNumber;

    public Slider expSlider;


    private int maxExp = 10000;


    public TextMeshProUGUI textgold;
    public TextMeshProUGUI textLevel;
    // public TextMeshProUGUI textMasaAtk1Level;
   
    // public TextMeshProUGUI textMasaAtk3Level;


    // public TextMeshProUGUI textMasaAtk1LevelUpGold;
    // public TextMeshProUGUI textMasaAtk3LevelUpGold;

    // 스킬 Ui
    public GameObject skillPrefab;
    //스킬 담을 패널 
    public Transform contentPanel;

    //////////////////////

    //스킬 정보가 담긴딕셔너리
    public Dictionary<string, Skill> skillDictionary = new();
    //스킬 Ui패널에 추가된 스킬들 
    public Dictionary<string, GameObject> skillObjectDictionary = new();
   
    void Awake()
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


    // Start is called before the first frame update
    void Start()
    {
        playerData = DataManager.dataManager.LoadPlayerData();

       
        selectedIndex = 0;
        
        //활성화된 캐릭터 
        SetActiveCharacter((int)selectCharacter);
        
        // skills 배열에 저장된 스킬
        if(playerData.skills != null)
        {
            foreach(var _skill in playerData.skills)
            {
                SetSkill(new Skill(_skill.whoSkill, _skill.skillName, _skill.level));
               
            }
       

        }

         
        
        CreateSkill();
        SetPlayerData();
       
    }

    //데이터 불러오기 
    public void SetPlayerData()
    {
        

        if(playerData != null)
        {
            SetLevel(playerData.level);
            SetExp(playerData.exp); 
            SetGold(playerData.gold);
            // masaAtk1Level = GetSkillLevel("Masa","Atk1",playerData.skills);
            // masaAtk3Level = GetSkillLevel("Masa","Atk3",playerData.skills);
            int _skillsLength = playerData.skills.Length;

            for(int i=0;i<_skillsLength;i++)
            {
                Skill _skill = playerData.skills[i];
                print("level" + _skill.level);
                SetSkillLevel(_skill);
       
            }

        }
        else
        {

            SetLevel(1);
            SetExp(0);
            SetGold(1000);
            // masaAtk1Level = 1;
            // masaAtk3Level = 1;

        }    
        
        
    
    }
    // public void UpdateSkillText()
    // {
    //     SetSkillLevel(ref masaAtk1Level,ref textMasaAtk1Level,ref textMasaAtk1LevelUpGold);
    //     SetSkillLevel(ref masaAtk3Level,ref textMasaAtk3Level, ref textMasaAtk3LevelUpGold);

    // }
    
    // public void Atk1SkillLevelUp()
    // {
    //     SetSkillLevel(ref masaAtk1Level,ref textMasaAtk1Level,ref textMasaAtk1LevelUpGold,true);
        

    // }

    // public void Atk3SkillLevelUp()
    // {
    //     SetSkillLevel(ref masaAtk3Level,ref textMasaAtk3Level, ref textMasaAtk3LevelUpGold,true);
    // }

    // public void SetSkillLevel(ref int _level,ref TextMeshProUGUI _textLevel,ref TextMeshProUGUI _textGold,bool _levelUp = false)
    // {
        
    //     int _gold = (int)(500*(_level*1.1f)* (_level*2));

    //     if(UseGold(_gold) && _levelUp)
    //     {
    //         _level += 1;
    //         _textLevel.text = _level.ToString();
    //         _textGold.text = _gold.ToString();
            
            
    //     }
    //     else
    //     {
    //         _textLevel.text = _level.ToString();
    //         _textGold.text = _gold.ToString();

    //     }
       

        
       
    // }
   

    public void SetLevel(int _level)
    {
        level = _level;
        textLevel.text = level.ToString();
        maxExp = maxExp + (level*1000);
    }

    public void SetGold(int _gold)
    {
        gold = _gold;
        textgold.text = gold.ToString();
    }

    public void SetExp(int _exp)
    {
        exp = _exp;
        expSlider.value = (float)exp/(float)maxExp;
    }

    public void SetSceneNumber(int _sceneNumber)
    {
        sceneNumber = _sceneNumber;

    }

    // Update is called once per frame
    void Update()
    {   
        // 솔저로 
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectCharacter = SelectCharacter.Solider;
            
            SetActiveCharacter((int)SelectCharacter.Solider);
            
            camPos.localPosition = new Vector3(0.05f,0.5f,0.3f);
            playerMove.CharacterReset();
            
           
        }
        // 마사 캐릭터로 
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectCharacter = SelectCharacter.MasaSchool;
            SetActiveCharacter((int)SelectCharacter.MasaSchool);
            camPos.localPosition = new Vector3(-0.03f,0.45f,-0.8f);
            
            playerMove.CharacterReset();
            
            
        }

        if(Input.GetKey(KeyCode.Alpha8))
        {
            NewSkill();

        }

        if(gold <= 30)
        {
            gold += 30;
        }


       
    }

    

    // 골드 획득시
    public void AddGold(int _gold)
    {
        gold += _gold;
       
        textgold.text = gold.ToString();
        
    }

    //골드 사용시 
    public bool UseGold(int _use)
    {
        if(0 > gold - _use)
            return false;

        gold -= _use;
        textgold.text = gold.ToString();
       

        return true;
    }

    //경험치 획득시 
    public void AddExp(int _exp)
    {
        exp += _exp;
       

        LevelUp();

        expSlider.value = (float)exp/(float)maxExp;

    }

    // 레벨업시 
    private void LevelUp()
    {
        
        if(exp >= maxExp)
        {
            
            exp -= maxExp;
            level++;

           

            maxExp += level*1000;
            textLevel.text = level.ToString();
            expSlider.value = (float)exp/(float)maxExp;
            AddGold(level*1000);

            DataManager.dataManager.SavePlayerData();
        }
    }

    // 현재 고른 캐릭터 
    public void SetActiveCharacter(int _index)
    {
        aim.SetActive(false);
        // 배열에 있는 모든 오브젝트를 비활성화
        for (int i = 0; i < characterMode.Length; i++)
        {
            characterMode[i].SetActive(false);
           
        }

        // 선택된 인덱스가 유효한 경우 해당 오브젝트를 활성화
        if (_index >= 0 && _index < characterMode.Length)
        {
            characterMode[_index].SetActive(true);
            selectedIndex = _index;

            if((int)SelectCharacter.Solider == _index)
            {
                aim.SetActive(true);
            }
           
            
        }
    }
    // public int GetSkillLevel(string whoskill, string skillName, Skill[] skills)
    // {
    //     // Skill 배열이 유효한지 확인합니다.
    //     if (skills != null)
    //     {
    //         // Skill 배열을 반복하면서 주어진 whoskill과 skillName을 가진 스킬을 찾습니다.
    //         foreach (Skill skill in skills)
    //         {
    //             // 현재 스킬의 whoskill과 skillName이 주어진 값과 일치하는지 확인합니다.
    //             if (skill.whoSkill == whoskill && skill.skillName == skillName)
    //             {
    //                 // 일치하는 스킬을 찾으면 해당 스킬의 level 값을 반환합니다.
    //                 return skill.level;
    //             }
    //         }
    //     }

    //     // 일치하는 스킬을 찾지 못하면 기본값으로 1을 반환합니다.
    //     return 1;
    // }
    public int GetSkillLevel(string _key)
    {
        print(skillDictionary[_key].level);
       
        if(!skillDictionary.ContainsKey(_key))
            return -1;
       
        return skillDictionary[_key].level;
    }





    // 딕셔너리에 스킬 추가 
    void SetSkill(Skill _skill)
    {
        skillDictionary[_skill.whoSkill + _skill.skillName] = _skill;
    }

    public void NewSkill()
    {
        
        Skill _skill = new Skill("Masa","Atk1",15);
        AddSkill(_skill);

    }

    // 스킬 추가 
    public void AddSkill(Skill _skill,string _key = "")
    {
        if(_key == "")
            _key = _skill.whoSkill + _skill.skillName;
        
        
        if(skillDictionary.ContainsKey(_key))
            return;
        skillDictionary[_key] = _skill;
       



        GameObject skillWindow = Instantiate(skillPrefab,contentPanel);
        TextMeshProUGUI[] texts = skillWindow.GetComponentsInChildren<TextMeshProUGUI>();
        Button button = skillWindow.GetComponentInChildren<Button>();
        skillObjectDictionary[_key] = skillWindow;

        // 이미지 로드 및 할당
        Sprite skillImage = Resources.Load<Sprite>("Sprites/" + _key); // 이미지 파일 경로
        Transform imageTransform = skillWindow.transform.Find("SkillImage");
        if(imageTransform != null)
        {
            Image imageComponent = imageTransform.GetComponent<Image>();
            if(imageComponent != null)
            {
                if(skillImage != null)
                {
                    imageComponent.sprite = skillImage;
                }
            }
        }
       
       
        
        //texts[0].text = // LV:고정
        texts[(int)SkillText.Level].text = _skill.level.ToString(); // 레벨이 몇인지
        texts[(int)SkillText.SkillName].text = _skill.skillName; // 스킬 이름 
        //texts[3].text =  LevelUp 고정
        texts[(int)SkillText.Gold].text = "300"; // 몇 골드 드는지
        //texts[5].text =  // G 고정 
        // 버튼 클릭 이벤트 추가
        button.onClick.AddListener(() => SetSkillLevel(_skill,true));

       


    }

    

    // 딕셔너리에 추가된 스킬을 ui로 생성 
    void CreateSkill()
    {
        int i = 0;
        foreach(KeyValuePair<string,Skill> enrty in skillDictionary)
        {
            Skill _skill = enrty.Value;
            string _key = _skill.whoSkill+_skill.skillName;
            GameObject skillWindow = Instantiate(skillPrefab,contentPanel);
            TextMeshProUGUI[] texts = skillWindow.GetComponentsInChildren<TextMeshProUGUI>();
            Button button = skillWindow.GetComponentInChildren<Button>();
            skillObjectDictionary[_key] = skillWindow;

            // 이미지 로드 및 할당
            Sprite skillImage = Resources.Load<Sprite>("Sprites/" + _key); // 이미지 파일 경로

            Transform imageTransform = skillWindow.transform.Find("SkillImage");
            if (imageTransform != null)
            {
                Image imageComponent = imageTransform.GetComponent<Image>();
                if (imageComponent != null)
                {
                    if (skillImage != null)
                    {
                        imageComponent.sprite = skillImage;
                    }
                }
            }

            SkillInfo skillInfo = skillWindow.GetComponentInChildren<SkillInfo>();
            skillInfo._key = _key;

             
            _skill = playerData.skills[i++];
            //texts[0].text = // LV:고정
            texts[(int)SkillText.Level].text = _skill.level.ToString(); // 레벨이 몇인지
            texts[(int)SkillText.SkillName].text = _skill.skillName; // 스킬 이름 
            //texts[3].text =  LevelUp 고정
            texts[(int)SkillText.Gold].text = "300"; // 몇 골드 드는지
            //texts[5].text =  // G 고정 


            // 버튼 클릭 이벤트 추가
            button.onClick.AddListener(() => SetSkillLevel(_skill,true));
      
        }
    }

    void SetSkillLevel(Skill _skill,bool _levelUp = false) 
    {
        int _level = _skill.level;
        string _key = _skill.whoSkill+_skill.skillName;
        
        GameObject skillWindow = skillObjectDictionary[_skill.whoSkill+_skill.skillName];
        TextMeshProUGUI[] texts = skillWindow.GetComponentsInChildren<TextMeshProUGUI>();
         
        int _gold = (int)(500*(_level*1.1f)* (_level*2));
       
        if(!skillDictionary.ContainsKey(_key))
            return;
        if(UseGold(_gold) && _levelUp)
        {
            
            _skill.level += 1;
            skillDictionary[_key].level = _skill.level;

               
            // UI 요소의 텍스트를 변경합니다.
            texts[(int)SkillText.Level].text = _skill.level.ToString(); // 레벨이 몇인지
            //texts[2].text = skill.skillName; // 스킬 이름 
            texts[(int)SkillText.Gold].text = _gold.ToString(); // 몇골드 드는지
           
        }
        else
        {
            
            texts[(int)SkillText.Level].text = _level.ToString(); // 레벨이 몇인지
            
            texts[(int)SkillText.Gold].text = _gold.ToString(); // 몇골드 드는지

        }
    }  

    enum SkillText
    {
        Level = 1,
        SkillName = 2,
        Gold = 4,


    }

   
}
