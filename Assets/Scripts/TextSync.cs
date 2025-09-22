using UnityEngine;
using UnityEngine.UI;

public class TextSync : MonoBehaviour
{
    public string sourceObjectName; // Nombre del objeto que contiene el texto fuente
    public Text targetText;         // Texto que se sincroniza

    private Text sourceText;        // Referencia al texto del prefab

    void Update()
    {
        // Si aún no tenemos referencia, intentamos buscarla
        if (sourceText == null)
        {
            GameObject sourceObj = GameObject.Find(sourceObjectName);
            if (sourceObj != null)
            {
                sourceText = sourceObj.GetComponent<Text>();
            }
        }

        // Si tenemos ambas referencias, copiamos el texto
        if (sourceText != null && targetText != null)
        {
            targetText.text = sourceText.text;
        }
    }
}
