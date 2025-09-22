using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneselect : MonoBehaviour
{

	public string NombreDeLaEscena;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void CargarelstringdelaescenaxD()
	{
       SceneManager.LoadScene(NombreDeLaEscena);
	}
}