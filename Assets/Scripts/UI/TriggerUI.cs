using UnityEngine;

public class TriggerUI : MonoBehaviour
{
    public GameObject onOffUi;

    int rotSpeed = 0;
    void OnTriggerEnter(Collider other)
    {  
        
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            On();
            
        }
            
    }

    void OnTriggerExit(Collider other)
    {
        
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
           Off();
        }
        
    }
    public void On()
    {
        onOffUi.SetActive(!onOffUi.activeSelf);
        rotSpeed = GameManager.gameManager.rotSpeed;
        GameManager.gameManager.rotSpeed = 0;
        GameManager.gameManager.isUI = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;


    }

    public void Off()
    {
        onOffUi.SetActive(!onOffUi.activeSelf);
        GameManager.gameManager.rotSpeed = rotSpeed;
       
        GameManager.gameManager.isUI = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }
}
