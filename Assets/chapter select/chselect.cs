using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class chselect : MonoBehaviour
{
	[SerializeField] private string room;
	
    public void ButtonPressed()
    {
        Debug.Log("Button pressed!");
		SceneManager.LoadScene(room);
    }
}
