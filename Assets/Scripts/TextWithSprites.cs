using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextWithSprites : MonoBehaviour {
	public string Text;
	public float Distance;
	[Header("Numbers Prefab")]
	public GameObject N1;
	public GameObject N2;
	public GameObject N3;
	public GameObject N4;
	public GameObject N5;
	public GameObject N6;
	public GameObject N7;
	public GameObject N8;
	public GameObject N9;
	public GameObject N0;
	private float ActualDistance;
	
	void Start() {
		foreach (char caracter in Text)
		{
			if (caracter == '1')
			{
				// Instantiate the prefab at a specific position and rotation
				GameObject instantiatedObject = Instantiate(N1, new Vector3(0, 0, 5), Quaternion.identity);
                instantiatedObject.name = "InstantiatedPrefab";
			}
			if (caracter == '2')
			{
				Debug.Log("Una a xdddd");
			}
			if (caracter == '3')
			{
				Debug.Log("Una a xdddd");
			}
			if (caracter == '4')
			{
				Debug.Log("Una a xdddd");
			}
			if (caracter == '5')
			{
				Debug.Log("Una a xdddd");
			}
			if (caracter == '6')
			{
				Debug.Log("Una a xdddd");
			}
			if (caracter == '7')
			{
				Debug.Log("Una a xdddd");
			}
			if (caracter == '8')
			{
				Debug.Log("Una a xdddd");
			}
			if (caracter == '9')
			{
				Debug.Log("Una a xdddd");
			}
			if (caracter == '0')
			{
				Debug.Log("Una a xdddd");
			}
		}
	}
}
