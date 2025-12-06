using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int currentScore = 0;
    public TextMeshProUGUI scoreText;  // Arrastra aquí tu TextMeshPro de puntuación
    public TextMeshProUGUI timerText;  // Arrastra aquí tu TextMeshPro de cronómetro

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

    // Desglose de puntuación
    public int enemyPoints = 0;
    public int boxPoints = 0;
    public int levelPoints = 0;
    public int timeBonusPoints = 0;

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
        // Si hay scoreText asignado, hacerlo persistente
        if (scoreText != null)
        {
            DontDestroyOnLoad(scoreText.gameObject);
        }

        // Si hay timerText asignado, hacerlo persistente
        if (timerText != null)
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
            AddTimeBonusPoints(bonus);
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
            AddLevelPoints(points);
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

    // Métodos específicos para cada tipo de puntuación (para el desglose)
    public void AddEnemyPoints(int points)
    {
        enemyPoints += points;
        AddPoints(points);
    }

    public void AddBoxPoints(int points)
    {
        boxPoints += points;
        AddPoints(points);
    }

    public void AddLevelPoints(int points)
    {
        levelPoints += points;
        AddPoints(points);
    }

    public void AddTimeBonusPoints(int points)
    {
        timeBonusPoints += points;
        AddPoints(points);
    }

    public void ResetScore()
    {
        currentScore = 0;
        enemyPoints = 0;
        boxPoints = 0;
        levelPoints = 0;
        timeBonusPoints = 0;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Puntos:\n" + currentScore;
        }
    }
}