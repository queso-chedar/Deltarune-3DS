using UnityEngine;

public class cebollaevento : MonoBehaviour
{
    public string eventoEscuchado = "Cebolla";
    public bool cebolla;

    public void TryActivate(string trigger)
    {
        if (trigger == eventoEscuchado)
        {
            cebolla = true;
            Debug.LogWarning("¡Evento '" + trigger + "' activado! LA CEBOLLA.");
        }
    }

    void Update()
    {
        if (cebolla)
        {
            // Aquí va la lógica del evento activado
        }
    }
}