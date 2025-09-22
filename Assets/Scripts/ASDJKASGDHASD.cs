using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASDJKASGDHASD : MonoBehaviour
{
	public AudioClip PapiroSonido;
	void Start()
	{
		StartCoroutine(papirosexo(2.5f));

	}
	    private IEnumerator papirosexo(float delay)
    {
        yield return new WaitForSeconds(delay);
         AudioSource.PlayClipAtPoint (PapiroSonido, transform.position);
    }
}
