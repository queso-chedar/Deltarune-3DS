using System.Collections;
using UnityEngine;

public class DialogueEventManager : MonoBehaviour
{
    [Header("Referencias")]
    public Animator animator;
    public Animator papiroanimator;
    public Animator KrisAnimator;
    public Animator TextAnimator;
    public GameObject KrisControllerScript;
    [Tooltip("Arrastra aquí el GameObject que tiene el script Lol (el de npc)")]
    public Lol lolScript;
    public GameObject ObjetoPapiroSonido;
    public FadeController fadeController;

    void Start()
    {
        DialogueSystem dialogueSystem = FindObjectOfType<DialogueSystem>();
        if (dialogueSystem != null)
        {
            dialogueSystem.OnEventTriggered += HandleEvent;
        }
    }

    void HandleEvent(string eventName)
    {
        Debug.Log("Evento recibido: " + eventName);

        if (eventName == "Inicio")
        {
            KrisControllerScript.GetComponent<KrisController>().enabled = false;
        }
        else if (eventName == "Fin")
        {
            StartCoroutine(EnableKrisWithDelay(0.1f));
            StartCoroutine(RemoveDialog(0.1001f));
        }
        else if (eventName == "COCK")
        {
            animator.Play("unzip");
        }
        else if (eventName == "papiro")
        {
            papiroanimator.Play("PAPIRO");
            ObjetoPapiroSonido.SetActive(true);
        }
        else if (eventName == "textogaster")
        {
            fadeController.FadeTo(0f);
        }
        else if (eventName == "textogasterinicio")
        {
            fadeController.SetAlphaInstant(1f);
        }
    }

    private IEnumerator EnableKrisWithDelay(float delay)
    {
        // Espera 0.1 segundos
        yield return new WaitForSeconds(delay);
        // Activa el componente
        KrisControllerScript.GetComponent<KrisController>().enabled = true;
    }
    private IEnumerator RemoveDialog(float delay)
    {
        // Espera 0.1 segundos
        yield return new WaitForSeconds(delay);
        // Activa el componente
        lolScript.RemoveCurrentDialog();
    }
}