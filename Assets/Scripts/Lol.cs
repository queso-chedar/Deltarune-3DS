using UnityEngine;

public class Lol : MonoBehaviour
{
    [Header("Diálogos iniciales (si no se asignan en Inspector, se buscan en los hijos)")]
    public GameObject[] Dialogos;

    [Header("Prefab del diálogo final (para duplicar)")]
    public GameObject dialogoFinalPrefab;
    private int dialogoActual = 0;
    private GameObject dialogoFinalInstancia;
	

    void Awake()
    {
        if (Dialogos == null || Dialogos.Length == 0)
        {
            int count = transform.childCount;
            Dialogos = new GameObject[count];
            for (int i = 0; i < count; i++)
            {
                Dialogos[i] = transform.GetChild(i).gameObject;
                Dialogos[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// Activa el siguiente diálogo o duplica el diálogo final.
    /// </summary>
    public void AdvanceDialog()
    {
        if (dialogoActual < Dialogos.Length)
        {
            GameObject dialogo = Dialogos[dialogoActual];
            dialogo.SetActive(true);
            dialogoActual++;
        }
        else
        {
            if (dialogoFinalInstancia != null)
                Destroy(dialogoFinalInstancia);

            dialogoFinalInstancia = Instantiate(dialogoFinalPrefab, transform);
            dialogoFinalInstancia.SetActive(true);
        }
    }

    /// <summary>
    /// Elimina el último diálogo activado (array o instancia final).
    /// </summary>
    public void RemoveCurrentDialog()
    {
        if (dialogoActual > 0)
        {
            int index = dialogoActual - 1;
            if (index < Dialogos.Length)
                Destroy(Dialogos[index]);
            else if (dialogoFinalInstancia != null)
                Destroy(dialogoFinalInstancia);
        }
    }
}