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
        SceneManager.LoadScene(room, LoadSceneMode.Single);
    }
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == targetButton.gameObject)
        {
            if (UnityEngine.N3DS.GamePad.GetButtonTrigger(N3dsButton.A) || Input.GetKeyDown(KeyCode.Z))
            {
                ButtonPressed();
            }
        }
    }
}
