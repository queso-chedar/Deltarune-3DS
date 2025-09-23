using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class chselect : MonoBehaviour
{
    [SerializeField] private string room;
    [SerializeField] private Button targetButton;

    public void ButtonPressed()
    {
        Debug.Log("Button pressed!");
        SceneManager.LoadScene(room);
    }
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == targetButton.gameObject)
        {
            Debug.Log(targetButton.name + " is highlighted via navigation");
            
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Z))
            {
                ButtonPressed();
            }
        }
    }
}
