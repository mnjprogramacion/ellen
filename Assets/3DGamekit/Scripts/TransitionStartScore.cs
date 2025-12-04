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
            scored = true;
            if (ScoreManager.Instance != null)
            {
                // Sumar puntos base por completar el nivel
                Debug.Log($"TransitionStartScore: Sumando {levelCompletePoints} puntos base por completar nivel.");
                ScoreManager.Instance.AddLevelCompletePoints(levelCompletePoints);
                
                // Sumar bonus de tiempo
                int timeBonus = ScoreManager.Instance.ClaimTimeBonus();
                Debug.Log($"TransitionStartScore: Bonus de tiempo sumado: {timeBonus}");
                
                // Reiniciar el cronómetro para el siguiente nivel
                ScoreManager.Instance.ResetLevelTimer();
                
                Debug.Log($"Después de completar nivel. Score total: {ScoreManager.Instance.currentScore}");
            }
            // NO destruir el gameObject ya que TransitionPoint lo necesita
            // Destroy(gameObject);
        }
    }
}
