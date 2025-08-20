using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public AudioClip hojaRecogidaClip;
    public AudioClip libroRecogidoClip;
    private AudioSource audioSource;

    public Transform spawnPoint;
    public GameObject PlayerCharacter;
    public static GameManager Instance;

    public int totalCollected = 0;
    public TMP_Text collectedText;

    public GameObject canvasPrefab;
    private static GameObject currentCanvas;

    public int pagesNeeded = 6;

    public GameObject notEnoughPagesPanel;
    public TMP_Text remainingPagesText;
    public GameObject bookPopupPanel;

    public bool gameFailed = false;
    public bool gameWon = false;
    public bool bookPickedUp = false;

    public GameObject VictoryPanel;
    public GameObject GameOverPanel;

    public void ExitGame()
    {
        Debug.Log("Saliendo del juego...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    private bool playerWasPositionedByPortal = false;

    public void MarkPlayerPositioned()
    {
        playerWasPositionedByPortal = true;
    }

    public void RestartGame()
    {
        ResetGameState();
        totalCollected = 0;

        if (VictoryPanel != null) VictoryPanel.SetActive(false);
        if (GameOverPanel != null) GameOverPanel.SetActive(false);
        if (bookPopupPanel != null) bookPopupPanel.SetActive(false);
        if (notEnoughPagesPanel != null) notEnoughPagesPanel.SetActive(false);

        Debug.Log("Restarting game — reloading scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResetGameState()
    {
        gameWon = false;
        gameFailed = false;
        bookPickedUp = false;
    }

    public bool CanPickUpBook()
    {
        return totalCollected >= pagesNeeded;
    }

    public int PagesRemaining()
    {
        return Mathf.Max(0, pagesNeeded - totalCollected);
    }

    public void UpdateSpawnPoint(Transform newSpawn)
    {
        spawnPoint = newSpawn;
    }

    public void AddToCount(int value)
    {
        totalCollected += value;
        UpdateUI();

        if (hojaRecogidaClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(hojaRecogidaClip);
        }

    }
    private void UpdateUI()
    {
        if (collectedText != null)
            collectedText.text = "HOJAS RECOGIDAS... " + totalCollected;
    }

    private void Awake()
    {
        DontDestroyOnLoad(PlayerCharacter);
        Debug.Log("GameManager Awake in scene: " + SceneManager.GetActiveScene().name);


        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;

            if (currentCanvas == null)
            {
                currentCanvas = Instantiate(canvasPrefab);
                DontDestroyOnLoad(currentCanvas); //esto es para que se traiga el canvas

                Debug.Log("Canvas instantiated in Awake");

            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        TopdownPlayer.CallPlayerDead += OnJugadorMuerto;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        TopdownPlayer.CallPlayerDead -= OnJugadorMuerto;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnJugadorMuerto()
    {
        StartCoroutine(RespawnRoutine());
    }

    public IEnumerator RespawnRoutine()
    {
        PlayerCharacter.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        PlayerCharacter.transform.position = spawnPoint.position;
        PlayerCharacter.SetActive(true);
    }

    private Button FindButtonInCanvas(string buttonName)
    {
        Button[] allButtons = currentCanvas.GetComponentsInChildren<Button>(true); // true incluye hasta paneles desactivados
        foreach (Button btn in allButtons)
        {
            if (btn.name == buttonName)
                return btn;
        }
        return null;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Button victoryRestartButton = FindButtonInCanvas("VictoryRestartButton");
        if (victoryRestartButton != null)
        {
            victoryRestartButton.onClick.RemoveAllListeners();
            victoryRestartButton.onClick.AddListener(RestartGame);
            Debug.Log("todo gucci");
        }

        else
        {
            Debug.LogWarning("game over restart no esta");
        }

        // Botón de salir en VictoryPanel
        Button victoryExitButton = FindButtonInCanvas("VictoryExitButton");
        if (victoryExitButton != null)
        {
            victoryExitButton.onClick.RemoveAllListeners();
            victoryExitButton.onClick.AddListener(ExitGame);
            Debug.Log("VictoryExitButton conectado");
        }
        else
        {
            Debug.LogWarning(" victory button exit no encontrado");
        }


        Button gameOverRestartButton = FindButtonInCanvas("GameOverRestartButton");
        if (gameOverRestartButton != null)
        {
            gameOverRestartButton.onClick.RemoveAllListeners();
            gameOverRestartButton.onClick.AddListener(RestartGame);
            Debug.Log("todo gucci");
        }

        else
        {
            Debug.LogWarning("game over restart no esta");
        }

        // Botón de salir en GameOverPanel
        Button gameOverExitButton = FindButtonInCanvas("GameOverExitButton");
        if (gameOverExitButton != null)
        {
            gameOverExitButton.onClick.RemoveAllListeners();
            gameOverExitButton.onClick.AddListener(ExitGame);
            Debug.Log("GameOverExitButton conectado");
        }
        else
        {
            Debug.LogWarning("game over exit no encontrado");
        }


        VictoryPanel = currentCanvas.transform.Find("VictoryPanel")?.gameObject;
        GameOverPanel = currentCanvas.transform.Find("GameOverPanel")?.gameObject;
        Debug.Log("GameOverPanel found: " + (GameOverPanel != null));


        if (VictoryPanel != null) VictoryPanel.SetActive(false);
        if (GameOverPanel != null) GameOverPanel.SetActive(false);

        collectedText = currentCanvas.transform.Find("CollectedCounter")?.GetComponent<TMP_Text>();
        notEnoughPagesPanel = currentCanvas.transform.Find("NotEnoughPagesPanel")?.gameObject;
        remainingPagesText = currentCanvas.transform.Find("NotEnoughPagesPanel/RemainingPagesText")?.GetComponent<TMP_Text>();
        bookPopupPanel = currentCanvas.transform.Find("BookPopupPanel")?.gameObject;

        Debug.Log("Panels found:");
        Debug.Log("NotEnoughPagesPanel: " + (notEnoughPagesPanel != null));
        Debug.Log("RemainingPagesText: " + (remainingPagesText != null));
        Debug.Log("BookPopupPanel: " + (bookPopupPanel != null));

        if (notEnoughPagesPanel != null) notEnoughPagesPanel.SetActive(false);
        if (bookPopupPanel != null) bookPopupPanel.SetActive(false);

        // nuevo spawn point
        spawnPoint = GameObject.Find("SpawnPoint")?.transform;

        if (spawnPoint == null)
            Debug.LogError("SpawnPoint not found in scene!");

        // reposiciona al jugador
        /*if (PlayerCharacter != null && spawnPoint != null)
        {
            PlayerCharacter.transform.position = spawnPoint.position;
            PlayerCharacter.SetActive(true);
        }*/

        if (PlayerCharacter != null && spawnPoint != null && !playerWasPositionedByPortal)
        {
            PlayerCharacter.transform.position = spawnPoint.position;
            PlayerCharacter.SetActive(true);
        }
        playerWasPositionedByPortal = false; // reset para futuras escenas


        if (currentCanvas != null)
        {
            collectedText = currentCanvas.transform.Find("CollectedCounter")?.GetComponent<TMP_Text>();
            UpdateUI();
        }

        //ok so antes se uso game object.find(collected counter), pero gameobject.find solo busca
        //objetos loaded en la current scene, no adentro de dont destroy on load, como es el 
        //prefab del canvas, asi que por eso no se actualizaba porque no lo veia, pero ahora con
        //el current canvas, el codigo le esta diciendo a unity que busque dentro del canvas
        //que se trajo desde la escena anterior

    }
    public void ShowNotEnoughPages()
    {
        if (notEnoughPagesPanel != null && remainingPagesText != null)
        {
            remainingPagesText.text = $"Me faltan {PagesRemaining()} hojas!!";
            notEnoughPagesPanel.SetActive(true);
            StartCoroutine(HideAfterDelay(notEnoughPagesPanel, 2f));
        }
    }

    public void ShowBookPopup()
    {
        if (bookPopupPanel != null)
            bookPopupPanel.SetActive(true);

        bookPickedUp = true;

        if (libroRecogidoClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(libroRecogidoClip);
        }

        if (CanPickUpBook())
        {
            gameWon = true;
        }

    }
    private IEnumerator HideAfterDelay(GameObject panel, float delay)
    {
        yield return new WaitForSeconds(delay);
        panel.SetActive(false);
    }

}



