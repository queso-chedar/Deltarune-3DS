using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spanishorenglish : MonoBehaviour {
	public bool Español = false;
	public Text TextoACambiar;
	[Header("TextoEspañol")]
	[TextArea]
	public string TextoEspañol;
	private string textoingles;

	// Use this for initialization
	void Start () {
		 textoingles = TextoACambiar.text;
	}

	// Update is called once per frame
	void Update()
	{
		if (Español == true)
		{
			TextoACambiar.text = TextoEspañol;
		}
		else
		{
			TextoACambiar.text = textoingles;
		}
	}
}
