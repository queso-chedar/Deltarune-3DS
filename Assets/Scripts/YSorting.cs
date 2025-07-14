using UnityEngine;
using System.Collections.Generic;

public class YSorting : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Precisión del ordenamiento (mayor valor = más separación entre objetos)")]
    public float precision = 100f;
    
    [Tooltip("Offset base para el personaje")]
    public int playerBaseOrder = 1000;
    
    [Header("Objetos para controlar")]
    public List<GameObject> sortingObjects = new List<GameObject>();
    
    private SpriteRenderer playerRenderer;
    private Dictionary<SpriteRenderer, int> baseOrders = new Dictionary<SpriteRenderer, int>();
    
    void Start()
    {
        playerRenderer = GetComponent<SpriteRenderer>();
        
        if (playerRenderer == null)
        {
            Debug.LogError("ERROR: El personaje necesita un SpriteRenderer");
            return;
        }
        
        // Registrar los órdenes base de todos los objetos
        foreach (GameObject obj in sortingObjects)
        {
            if (obj != null)
            {
                SpriteRenderer objRenderer = obj.GetComponent<SpriteRenderer>();
                if (objRenderer != null)
                {
                    baseOrders[objRenderer] = objRenderer.sortingOrder;
                }
            }
        }
    }
    
    void LateUpdate()
    {
        // Calcular el orden para el personaje basado en su posición Y global
        playerRenderer.sortingOrder = playerBaseOrder + Mathf.RoundToInt(transform.position.y * -precision);
        
        // Calcular el orden para cada objeto basado en su posición Y global
        foreach (KeyValuePair<SpriteRenderer, int> entry in baseOrders)
        {
            SpriteRenderer renderer = entry.Key;
            int baseOrder = entry.Value;
            
            renderer.sortingOrder = baseOrder + Mathf.RoundToInt(renderer.transform.position.y * -precision);
        }
    }
    
    // Método para añadir objetos dinámicamente
    public void AddSortingObject(GameObject newObj)
    {
        if (!sortingObjects.Contains(newObj))
        {
            sortingObjects.Add(newObj);
            
            SpriteRenderer objRenderer = newObj.GetComponent<SpriteRenderer>();
            if (objRenderer != null)
            {
                baseOrders[objRenderer] = objRenderer.sortingOrder;
            }
        }
    }
    
    // Método para debuggear
    public void PrintCurrentOrders()
    {
        Debug.Log("=== ÓRDENES ACTUALES ===");
        
        foreach (KeyValuePair<SpriteRenderer, int> entry in baseOrders)
        {
            SpriteRenderer renderer = entry.Key;
        }
    }
}