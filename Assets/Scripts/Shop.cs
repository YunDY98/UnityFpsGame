using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    
    string itemName;

    int price;

    string whoSkill;
    string skillName;

    Button btn;
    Skill skill = new("","",1);

    string key;
    
    // Start is called before the first frame update
    void Start()
    {
       
        ItemInfo itemInfo = GetComponent<ItemInfo>();

        price = itemInfo.price;
        itemName = itemInfo.itemName;

        whoSkill = itemInfo.who;
        skillName = itemInfo.skillName;


        key = whoSkill + skillName;

        print(key);


        // 첫번째 UGUI가 Price
        TextMeshProUGUI _textPrice = GetComponentInChildren<TextMeshProUGUI>(); 
        _textPrice.text = price.ToString();

       
        // 스킬 정보 
        skill.whoSkill = whoSkill;
        skill.skillName = skillName;
        skill.level = 1;

        // 버튼 추가 
        btn = GetComponent<Button>();   

        btn.onClick.AddListener(Buy);
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Buy()
    {
        try
        {
            
            skill = PlayerStats.playerStats.skillDictionary[key];
            if(skill != null )
            {
                
                // 이미 보유중인 아이템입니다 
                return;
            }
            
        }
        catch
        {

            if(!PlayerStats.playerStats.UseGold(price))
            {
                //돈이 부족합니다 
                return;

            }

            PlayerStats.playerStats.AddSkill(skill,key);

        }

        

        

        
        
       

        
        
        
        

       
        
    }
}
