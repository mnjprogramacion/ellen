using UnityEngine;
using Gamekit3D;

public class ButtonTransition : MonoBehaviour
{
    [Header("Referencias")]
    public TransitionPoint transitionPoint;  // Arrastra aquí el TransitionPoint de la escena

    // Llama a este método desde el evento OnClick del botón
    public void OnButtonClick()
    {
        if (transitionPoint != null)
        {
            Debug.Log("Botón presionado. Activando transición...");
            
            // Reanudar el tiempo si estaba pausado
            Time.timeScale = 1f;
            
            // Llamar al método Transition del TransitionPoint
            transitionPoint.Transition();
        }
        else
        {
            Debug.LogWarning("ButtonTransition: No se ha asignado un TransitionPoint.");
        }
    }
}
