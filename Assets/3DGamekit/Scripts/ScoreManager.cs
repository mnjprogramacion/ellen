using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int currentScore = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    private GameObject canvasObj;

    // Prefabs de referencia para asignar en el inspector
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public GameObject boxPrefab;

    // Control para evitar sumar puntos de nivel más de una vez por escena
    private int lastSceneLevelScore = -1;

    // Sistema de bonus por tiempo
    public int startingTimeBonus = 1000;      // Bonus inicial
    public int timeBonusPenaltyPerMinute = 100; // Puntos que se restan por minuto
    private float levelStartTime;              // Tiempo cuando empezó el nivel
    private int currentTimeBonus;              // Bonus actual

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null); // Asegurar que sea objeto raíz para que DontDestroyOnLoad funcione correctamente
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Si no hay scoreText asignado, lo crea automáticamente en la UI
        canvasObj = GameObject.Find("Canvas");
        if (scoreText == null)
        {
            if (canvasObj == null)
            {
                canvasObj = new GameObject("Canvas");
                var canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
                canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
                DontDestroyOnLoad(canvasObj); // Hacer persistente el Canvas
            }
            else
            {
                DontDestroyOnLoad(canvasObj); // Si ya existe, hacerlo persistente
            }

            GameObject textObj = new GameObject("ScoreText");
            textObj.transform.SetParent(canvasObj.transform);
            scoreText = textObj.AddComponent<TextMeshProUGUI>();
            scoreText.fontSize = 36;
            scoreText.alignment = TextAlignmentOptions.TopRight;
            scoreText.rectTransform.anchorMin = new Vector2(1, 1);
            scoreText.rectTransform.anchorMax = new Vector2(1, 1);
            scoreText.rectTransform.pivot = new Vector2(1, 1);
            scoreText.rectTransform.anchoredPosition = new Vector2(-20, -20);
            DontDestroyOnLoad(textObj); // Hacer persistente el ScoreText
        }
        else
        {
            DontDestroyOnLoad(scoreText.gameObject); // Si ya existe, hacerlo persistente
        }

        // Crear el texto del cronómetro
        if (timerText == null)
        {
            GameObject timerObj = new GameObject("TimerText");
            timerObj.transform.SetParent(canvasObj.transform);
            timerText = timerObj.AddComponent<TextMeshProUGUI>();
            timerText.fontSize = 36;
            timerText.alignment = TextAlignmentOptions.TopRight;
            timerText.rectTransform.anchorMin = new Vector2(1, 1);
            timerText.rectTransform.anchorMax = new Vector2(1, 1);
            timerText.rectTransform.pivot = new Vector2(1, 1);
            timerText.rectTransform.anchoredPosition = new Vector2(-20, -100); // Debajo del score
            DontDestroyOnLoad(timerObj);
        }
        else
        {
            DontDestroyOnLoad(timerText.gameObject);
        }

        // Iniciar el cronómetro del nivel
        ResetLevelTimer();
        UpdateScoreUI();
    }

    private void Update()
    {
        // Simulación de eventos de puntuación con teclas SOLO para bonus
        if (Input.GetKeyDown(KeyCode.B)) // Bonus por rapidez
            AddPoints(200);

        if (Input.GetKeyDown(KeyCode.R)) // Reiniciar puntuación
            ResetScore();

        // Actualizar el cronómetro y el bonus de tiempo
        UpdateTimerUI();
    }

    // Reinicia el cronómetro al empezar un nuevo nivel
    public void ResetLevelTimer()
    {
        levelStartTime = Time.time;
        currentTimeBonus = startingTimeBonus;
        Debug.Log($"Cronómetro reiniciado. Bonus inicial: {currentTimeBonus}");
    }

    // Obtiene el tiempo transcurrido en el nivel actual
    public float GetLevelTime()
    {
        return Time.time - levelStartTime;
    }

    // Calcula el bonus de tiempo actual basado en el tiempo transcurrido
    public int GetCurrentTimeBonus()
    {
        float elapsedMinutes = GetLevelTime() / 60f;
        int penalty = Mathf.FloorToInt(elapsedMinutes) * timeBonusPenaltyPerMinute;
        currentTimeBonus = Mathf.Max(0, startingTimeBonus - penalty);
        return currentTimeBonus;
    }

    // Suma el bonus de tiempo actual a la puntuación
    public int ClaimTimeBonus()
    {
        int bonus = GetCurrentTimeBonus();
        if (bonus > 0)
        {
            Debug.Log($"Bonus de tiempo reclamado: {bonus} puntos");
            AddPoints(bonus);
        }
        return bonus;
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            float elapsed = GetLevelTime();
            int minutes = Mathf.FloorToInt(elapsed / 60f);
            int seconds = Mathf.FloorToInt(elapsed % 60f);
            int timeBonus = GetCurrentTimeBonus();
            timerText.text = $"Tiempo:\n{minutes:00}:{seconds:00}\nBonus:\n{timeBonus}";
        }
    }

    // Método para sumar puntos de nivel solo una vez por escena
    public void AddLevelCompletePoints(int points)
    {
        int sceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        Debug.Log($"AddLevelCompletePoints llamado. Puntos: {points}, SceneIndex: {sceneIndex}, LastSceneLevelScore: {lastSceneLevelScore}");
        if (lastSceneLevelScore != sceneIndex)
        {
            lastSceneLevelScore = sceneIndex;
            AddPoints(points);
        }
        else
        {
            Debug.Log("Puntos de nivel ya sumados para esta escena.");
        }
    }

    public void AddPoints(int points)
    {
        Debug.Log($"AddPoints llamado. Sumando {points} a currentScore {currentScore}. Nuevo total: {currentScore + points}");
        currentScore += points;
        UpdateScoreUI();
    }

    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Puntuación: " + currentScore;
        }
    }
}