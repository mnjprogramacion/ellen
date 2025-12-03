using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int currentScore = 0;
    public TextMeshProUGUI scoreText;
    private GameObject canvasObj;

    // Prefabs de referencia para asignar en el inspector
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public GameObject boxPrefab;

    // Control para evitar sumar puntos de nivel más de una vez por escena
    private int lastSceneLevelScore = -1;

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

        UpdateScoreUI();
    }

    private void Update()
    {
        // Simulación de eventos de puntuación con teclas SOLO para bonus
        if (Input.GetKeyDown(KeyCode.B)) // Bonus por rapidez
            AddPoints(200);

        if (Input.GetKeyDown(KeyCode.R)) // Reiniciar puntuación
            ResetScore();
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