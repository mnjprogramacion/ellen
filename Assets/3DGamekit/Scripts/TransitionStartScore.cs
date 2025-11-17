using UnityEngine;

public class TransitionStartScore : MonoBehaviour
{
    public int levelCompletePoints = 1000;
    private bool scored = false;

    private void OnTriggerEnter(Collider other)
    {
        // Comprueba si el objeto que entra es el jugador
        if (!scored && other.CompareTag("Player"))
        {
            scored = true; // Evita sumar puntos múltiples
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddPoints(levelCompletePoints);
            }
        }
    }
}
