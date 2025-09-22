using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneselectNOTUI : MonoBehaviour
{

	public string NombreDeLaEscena;

	// Use this for initialization
	void Start()
	{
SceneManager.LoadScene(NombreDeLaEscena, LoadSceneMode.Single);
	}

	// Update is called once per frame
	void Update()
	{
	    
	}
}