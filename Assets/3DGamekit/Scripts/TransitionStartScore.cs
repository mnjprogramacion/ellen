using UnityEngine;

public class TransitionStartScore : MonoBehaviour
{
    public int levelCompletePoints = 500;
    private bool scored = false;

    private void OnTriggerEnter(Collider other)
    {
        // Solo suma puntos la primera vez que el jugador entra
        if (!scored && other.CompareTag("Player"))
        {
            scored = true;
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddLevelCompletePoints(levelCompletePoints);
            }
            Destroy(gameObject); // El objeto se destruye tras sumar puntos
        }
    }
}
