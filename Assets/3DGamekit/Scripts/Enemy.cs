using UnityEngine;
using Gamekit3D;

public class Enemy : MonoBehaviour
{
    public int points = 100;
    private bool scored = false;

    private void Start()
    {
        // Buscar el componente Damageable en este objeto o en sus hijos
        Damageable damageable = GetComponentInChildren<Damageable>();
        if (damageable != null)
        {
            damageable.OnDeath.AddListener(OnEnemyDeath);
            Debug.Log($"Enemy: Damageable encontrado en {damageable.gameObject.name}");
        }
        else
        {
            Debug.LogWarning($"Enemy: No se encontró componente Damageable en {gameObject.name} ni en sus hijos");
        }
    }

    // Este método se llama cuando el enemigo muere por daño del jugador
    private void OnEnemyDeath()
    {
        if (!scored)
        {
            scored = true;
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddEnemyPoints(points);
                Debug.Log($"Enemigo {gameObject.name} muerto. Sumando {points} puntos.");
            }
        }
    }
}
