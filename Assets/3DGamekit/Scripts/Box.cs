using UnityEngine;

public class Box : MonoBehaviour
{
    public int points = 300;
    private bool opened = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!opened && other.CompareTag("Player"))
        {
            opened = true;
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddPoints(points);
            }
            Destroy(gameObject); // El cofre desaparece al abrirse
        }
    }
}
