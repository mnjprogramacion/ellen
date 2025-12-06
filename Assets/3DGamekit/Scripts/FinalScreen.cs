using UnityEngine;
using TMPro;

public class FinalScreen : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject finalCanvas;           // El Canvas "Final" que se activará
    public TextMeshProUGUI finalScoreText;   // Texto para mostrar el desglose de puntuación

    [Header("Configuración")]
    public bool pauseGameOnFinish = true;    // Pausar el juego al mostrar la pantalla final

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // Detecta al jugador por tag "Player" o por nombre "Ellen"
        bool isPlayer = other.CompareTag("Player") || other.name == "Ellen";

        if (!triggered && isPlayer)
        {
            triggered = true;
            ShowFinalScreen();
        }
    }

    private void ShowFinalScreen()
    {
        Debug.Log("¡Juego completado! Mostrando pantalla final.");

        // Activar el Canvas final
        if (finalCanvas != null)
        {
            finalCanvas.SetActive(true);
        }

        // Actualizar el texto con el desglose de puntuación
        if (finalScoreText != null && ScoreManager.Instance != null)
        {
            // Sumar el último bonus de tiempo antes de mostrar
            int finalTimeBonus = ScoreManager.Instance.ClaimTimeBonus();

            // Obtener el desglose
            int enemyPoints = ScoreManager.Instance.enemyPoints;
            int boxPoints = ScoreManager.Instance.boxPoints;
            int levelPoints = ScoreManager.Instance.levelPoints;
            int timeBonusPoints = ScoreManager.Instance.timeBonusPoints;
            int totalScore = ScoreManager.Instance.currentScore;

            // Crear el texto con el desglose
            string breakdown = $"Enemigos:\t\t{enemyPoints}\n";
            breakdown += $"Cajas:\t\t{boxPoints}\n";
            breakdown += $"Niveles:\t\t{levelPoints}\n";
            breakdown += $"Bonus tiempo:\t{timeBonusPoints}\n";
            breakdown += "\n";
            breakdown += $"TOTAL PTS:\t{totalScore}\n";

            finalScoreText.text = breakdown;
        }

        // Ocultar la UI del juego (puntuación y cronómetro)
        if (ScoreManager.Instance != null)
        {
            if (ScoreManager.Instance.scoreText != null)
                ScoreManager.Instance.scoreText.gameObject.SetActive(false);
            if (ScoreManager.Instance.timerText != null)
                ScoreManager.Instance.timerText.gameObject.SetActive(false);
        }

        // Buscar y destruir el objeto ScoreManager
        GameObject scoreManagerObj = GameObject.Find("ScoreManager");
        if (scoreManagerObj != null)
        {
            Destroy(scoreManagerObj);
            Debug.Log("ScoreManager destruido.");
        }

        // Pausar el juego si está configurado
        if (pauseGameOnFinish)
        {
            Time.timeScale = 0f;
        }
    }

    // Método público para reiniciar el juego (puedes llamarlo desde un botón)
    public void RestartGame()
    {
        Time.timeScale = 1f;
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
        }
        // Cargar la primera escena del juego
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    // Método público para salir del juego (puedes llamarlo desde un botón)
    public void QuitGame()
    {
        Time.timeScale = 1f;
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
