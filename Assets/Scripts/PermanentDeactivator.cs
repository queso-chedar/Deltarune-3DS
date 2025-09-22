using UnityEngine;

public class PermanentDeactivator : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Objeto que se desactivará")]
    public GameObject objectToDeactivate;
    
    [Tooltip("Tiempo en segundos antes de desactivar el objeto")]
    public float timeToDeactivate = 5f;
    private float currentTime;
    private bool hasDeactivated = false;

    void Start()
    {
        currentTime = timeToDeactivate;
        
        // Asegurarse que el objeto está activo al inicio
        if (objectToDeactivate != null)
        {
            objectToDeactivate.SetActive(true);
        }
    }
    
    void Update()
    {
        if (hasDeactivated) return;
        
        // Reducir el tiempo
        currentTime -= Time.deltaTime;
        
        // Desactivar cuando el tiempo llegue a cero
        if (currentTime <= 0f)
        {
            DeactivatePermanently();
        }
    }
    
    void DeactivatePermanently()
    {
        if (objectToDeactivate != null)
        {
            objectToDeactivate.SetActive(false);
        }
        
        hasDeactivated = true;
        
        // Opcional: Deshabilitar este script después de desactivar el objeto
        enabled = false;
        
        Debug.Log("Objeto desactivado permanentemente");
    }
    
    // Método público para reiniciar manualmente
    public void ResetTimer()
    {
        currentTime = timeToDeactivate;
        hasDeactivated = false;
        enabled = true;
        
        if (objectToDeactivate != null)
        {
            objectToDeactivate.SetActive(true);
        }
    }
}