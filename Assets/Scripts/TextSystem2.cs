using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextSystem2 : MonoBehaviour
{
	string frase = "asdadqweda";
	public Text UIText;
	public float TextSpeed;
	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(Reloj());
	}

	IEnumerator Reloj()
	{
		foreach (char caracter in frase)
		{
			UIText.text = UIText.text + caracter;
			yield return new WaitForSeconds(TextSpeed);
		}
	}
}
		/* foreach es For Each = para cada uno, char es cada caracter
           asi que llamaremos char "caracter" cada caracter en frase, frase = el texto que vas a poner
		   texto.text (accedemos al componente texto del texto) y lo reemplazamos por el texto + caracter
		   eso we no es tan dificil pendejo
		*/