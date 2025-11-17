using UnityEngine;

public class Box : MonoBehaviour
{
    public int points = 300;

    private void OnDestroy()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddPoints(points);
        }
    }
}
