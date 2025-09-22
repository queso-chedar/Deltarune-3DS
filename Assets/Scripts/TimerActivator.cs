using UnityEngine;
using UnityEngine.UI; // Necesario si usas UI Text para mostrar el contador

public class TimerActivator : MonoBehaviour
{
    [Header("Configuración del Temporizador")]
    [Tooltip("Objeto que se activará/desactivará")]
    public GameObject objectToActivate;
    
    [Tooltip("Tiempo en segundos antes de activar el objeto")]
    public float timeToActivate = 5f;
    
    [Tooltip("Tiempo en segundos que el objeto permanece activo")]
    public float activeDuration = 3f;
    
    [Tooltip("Número de veces que se repite el ciclo (0 para infinito)")]
    public int repeatCount = 1;
    
    private float currentTime;
    private bool isObjectActive;
    private int currentRepeat;
    
    void Start()
    {
        // Asegurarse que el objeto está desactivado al inicio
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(false);
        }
        
        currentTime = timeToActivate;
        isObjectActive = false;
        currentRepeat = 0;
    }
    
    void Update()
    {
        // Reducir el tiempo
        currentTime -= Time.deltaTime;
        
        if (currentTime <= 0f)
        {
            if (!isObjectActive)
            {
                // Activar el objeto
                ActivateObject();
            }
            else
            {
                // Desactivar el objeto
                DeactivateObject();
            }
        }
    }
    
    void ActivateObject()
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }
        
        isObjectActive = true;
        currentTime = activeDuration;
        currentRepeat++;
        
        Debug.Log("Objeto activado. Repetición: " + currentRepeat);
    }
    
    void DeactivateObject()
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(false);
        }
        
        isObjectActive = false;
        
        // Verificar si debemos continuar el ciclo
        if (repeatCount == 0 || currentRepeat < repeatCount)
        {
            currentTime = timeToActivate;
        }
        else
        {
            // Deshabilitar este script si no hay más repeticiones
            enabled = false;
            Debug.Log("Ciclo de temporizador completado.");
        }
    }
    
    // Métodos públicos para control manual
    public void ResetTimer()
    {
        currentTime = timeToActivate;
        currentRepeat = 0;
        isObjectActive = false;
        
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(false);
        }
        
        enabled = true;
    }
    
    public void SetActiveDuration(float newDuration)
    {
        activeDuration = newDuration;
    }
    
    public void SetTimeToActivate(float newTime)
    {
        timeToActivate = newTime;
    }
}