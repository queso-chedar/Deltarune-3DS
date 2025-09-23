using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class battleselect : MonoBehaviour
{
    //[SerializeField] private string room;
    public GameObject battleBox;
    public GameObject soul;
    public GameObject playerrow;
    public GameObject self;
    public GameObject speakertext;
    public GameObject speakericon;
    public GameObject SAPMSTONSHOOOOOT;

    public void ButtonPressed()
    {
        Debug.Log("Button pressed!");
        battleBox.SetActive(true);
        playerrow.SetActive(false);
        soul.SetActive(true);
        self.SetActive(true);
        speakertext.SetActive(false);
        speakericon.SetActive(false);
        SAPMSTONSHOOOOOT.SetActive(true);
        //SceneManager.LoadScene(room);
    }
}
