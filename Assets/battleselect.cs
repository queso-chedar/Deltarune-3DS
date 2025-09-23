using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class battleselect : MonoBehaviour
{
    //[SerializeField] private string room;
    public GameObject battleBox;
    public GameObject soul;

    public void ButtonPressed()
    {
        Debug.Log("Button pressed!");
        battleBox.SetActive(true);
        soul.SetActive(true);
        //SceneManager.LoadScene(room);
    }
}
