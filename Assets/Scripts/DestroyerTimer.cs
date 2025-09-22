using UnityEngine;
using System.Collections;
public class DestroyerTimer : MonoBehaviour
{
    public GameObject ObjetoADestruir;
    public float TiempoADestruir;
    private IEnumerator coroutine;

    void Start()
    {
        coroutine = WaitAndPrint(TiempoADestruir);
        StartCoroutine(coroutine);
    }
    private IEnumerator WaitAndPrint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(ObjetoADestruir);
    }
}