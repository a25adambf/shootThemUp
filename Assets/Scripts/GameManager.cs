using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    const int LIVES = 3;
    const int EXTRA_LIFE_THRESHOLD = 100; // Puntos necesarios para obtener una vida extra

    [SerializeField] TextMeshProUGUI txtScore;
    [SerializeField] TextMeshProUGUI txtMaxScore;
    [SerializeField] TextMeshProUGUI txtMessage;
    //Array para las imágenes que marcan las vidas 
    [SerializeField] GameObject[] imgLives;

    int score;
    int maxScore;
    int nextExtraLife = EXTRA_LIFE_THRESHOLD; // Controla cuándo se otorga la siguiente vida extra
    //Inicializamos las vidas a la constante 
    int lives = LIVES;

    static GameManager instance;

    // Método estático para obtener la instancia del GameManager
    public static GameManager GetInstance()
    {
        return instance;
    }

    // Función Awake se ejecuta cuando se instancia el objeto
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);  Evitar que el objeto se destruya al cambiar de escena
        }
        else if (instance != this)
        {
            // Si ya existe una instancia, destruimos el nuevo GameManager para mantener la singularidad
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUI(); // Asegurar que la UI se actualice al inicio
        Debug.Log("GameManager iniciado. Vidas: " + lives + " | imgLives.Length: " + (imgLives != null ? imgLives.Length : 0));
    }

    void Update()
    {
        UpdateUI();
    }

    /// <summary>
    /// Actualiza toda la interfaz de usuario: vidas, puntuación y puntuación máxima.
    /// </summary>
    void UpdateUI()
    {
        // PRIMERO: Actualizar las imágenes de vidas (lo más importante)
        if (imgLives != null)
        {
            for (int i = 0; i < imgLives.Length; i++)
            {
                if (imgLives[i] != null)
                {
                    imgLives[i].SetActive(i < lives);
                }
            }
        }

        // SEGUNDO: Actualizar textos (con protección null)
        if (txtScore != null)
        {
            txtScore.text = string.Format("{0,4:D4}", score);
        }

        if (txtMaxScore != null)
        {
            txtMaxScore.text = string.Format("{0,4:D4}", maxScore);
        }
    }

    /// <summary>
    /// Añade puntos a la puntuación actual y verifica si se ha alcanzado una vida extra.
    /// </summary>
    /// <param name="points">Puntos a añadir</param>
    public void AddScore(int points)
    {
        score += points;
        Debug.Log("Puntuación: " + score);

        // Comprobar si se ha alcanzado la puntuación para una vida extra
        if (score >= nextExtraLife)
        {
            lives++;
            nextExtraLife += EXTRA_LIFE_THRESHOLD;
            Debug.Log("¡VIDA EXTRA! Vidas: " + lives);
            if (txtMessage != null)
            {
                txtMessage.text = "¡VIDA EXTRA!";
                Invoke(nameof(ClearMessage), 2f);
            }
        }

        // Actualizar la puntuación máxima
        if (score > maxScore)
        {
            maxScore = score;
        }
    }

    /// <summary>
    /// Resta una vida al jugador. Si las vidas llegan a 0, se ejecuta el Game Over.
    /// </summary>
    public void LoseLife()
    {
        lives--;
        Debug.Log("Vida perdida. Vidas restantes: " + lives);

        if (lives <= 0)
        {
            GameOver();
        }
    }

    /// <summary>
    /// Maneja el fin de la partida.
    /// </summary>
    void GameOver()
    {
        Debug.Log("GAME OVER");
         // Congelar todo el juego
        if (txtMessage != null)
        {
            txtMessage.gameObject.SetActive(true);   // Forzar activación del GameObject
            txtMessage.text = "GAME OVER";
        }
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Limpia el mensaje de la pantalla.
    /// </summary>
    void ClearMessage()
    {
        if (txtMessage != null)
        {
            txtMessage.text = "";
        }
    }

    /// <summary>
    /// Devuelve si el jugador sigue con vidas.
    /// </summary>
    public bool IsGameOver()
    {
        return lives <= 0;
    }
}