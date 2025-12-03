using UnityEngine;
using Gamekit3D;

public class Enemy : MonoBehaviour
{
    public int points = 100;
    private bool scored = false;

    private void Start()
    {
        // Buscar el componente Damageable y suscribirse al evento OnDeath
        Damageable damageable = GetComponent<Damageable>();
        if (damageable != null)
        {
            damageable.OnDeath.AddListener(OnEnemyDeath);
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
                ScoreManager.Instance.AddPoints(points);
                Debug.Log($"Enemigo {gameObject.name} muerto. Sumando {points} puntos.");
            }
        }
    }
}
