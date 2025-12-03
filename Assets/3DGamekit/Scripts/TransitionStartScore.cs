using UnityEngine;

public class TransitionStartScore : MonoBehaviour
{
    public int levelCompletePoints = 500;
    private bool scored = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"TransitionStartScore OnTriggerEnter. GameObject: {gameObject.name}, Other: {other.name}, Tag: {other.tag}, scored: {scored}");
        
        // Solo suma puntos la primera vez que el jugador entra
        // Detecta al jugador por tag "Player" o por nombre "Ellen"
        bool isPlayer = other.CompareTag("Player") || other.name == "Ellen";
        
        if (!scored && isPlayer)
        {
            Debug.Log($"TransitionStartScore activado en {gameObject.name}. Sumando {levelCompletePoints} puntos. Score actual: {ScoreManager.Instance?.currentScore}");
            scored = true;
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddLevelCompletePoints(levelCompletePoints);
                Debug.Log($"Después de AddLevelCompletePoints. Score: {ScoreManager.Instance.currentScore}");
            }
            // NO destruir el gameObject ya que TransitionPoint lo necesita
            // Destroy(gameObject);
        }
    }
}
