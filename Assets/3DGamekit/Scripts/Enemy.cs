using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int points = 100;

    private void OnDestroy()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddPoints(points);
        }
    }
}
