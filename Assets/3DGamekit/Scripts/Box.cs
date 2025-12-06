using UnityEngine;
using Gamekit3D;

public class Box : MonoBehaviour
{
    public int points = 300;
    private bool scored = false;

    private void Start()
    {
        // Buscar el componente Damageable en este objeto o en sus hijos
        Damageable damageable = GetComponentInChildren<Damageable>();
        
        if (damageable != null)
        {
            damageable.OnDeath.AddListener(OnBoxDestroyed);
            Debug.Log($"Box: Damageable encontrado en {damageable.gameObject.name}");
        }
        else
        {
            Debug.LogWarning($"Box: No se encontró componente Damageable en {gameObject.name} ni en sus hijos");
        }
    }

    // Este método se llama cuando la caja es destruida por el jugador
    private void OnBoxDestroyed()
    {
        if (!scored)
        {
            scored = true;
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddBoxPoints(points);
                Debug.Log($"Caja {gameObject.name} destruida. Sumando {points} puntos.");
            }
        }
    }
}
