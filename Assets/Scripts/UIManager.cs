using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesDisplay;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private Text _winText;
    [SerializeField]
    private Text _waveText;
    [SerializeField]
    private GameObject _pauseScreen;

    [SerializeField]
    private GameObject _playerPrefab;   

    [SerializeField]
    private Sprite[] _livesSprites;

    private SpawnManager _spawnManager;

    private int _currentScore = 0;

    private bool _isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + _currentScore;
        _livesDisplay.sprite = _livesSprites[3]; // Setting the UI to max lives
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogWarning("Could not find the Spawn Manager script");
        }
    }

    void Update()
    {
        if (_isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            _pauseScreen.SetActive(true);
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void UpdateScore()
    {
        _currentScore += 10;
        _scoreText.text = "Score: " + _currentScore;
    }
    
    public void DecreaseAmmo(int ammoCount)
    {
        _ammoText.text = "Ammo:" + ammoCount + "/15";
    }

    public void DecreaseLife(int lives)
    {
        _livesDisplay.sprite = _livesSprites[lives];

        if (lives <= 0)
        {
            _gameOverText.gameObject.SetActive(true);
            _restartText.gameObject.SetActive(true);
            _isGameOver = true;
            StartCoroutine(GameOverFlicker());
        }
    }

    public void AmmoRefilled()
    {
        _ammoText.text = "Ammo:15/15";
    }

    public void IncreaseLife(int lives)
    {
        // We can assume this will only called when the health power up is picked up (for now)
        _livesDisplay.sprite = _livesSprites[lives];
    }

    public void PlayerWon()
    {
        // Bring up Win Screen UI for the player
        _winText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        _isGameOver = true;
        StartCoroutine(WinFlicker());
    }

    public void ContinueGame()
    {
        _pauseScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UpdateWave(int currentWave)
    {
        _waveText.text = "Wave " + currentWave;
        StartCoroutine(WavePresentation());
    }

    IEnumerator GameOverFlicker()
    {

        while (_isGameOver)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.4f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.4f);
        }

    }

    IEnumerator WinFlicker()
    {

        while (_isGameOver)
        {
            _winText.text = "You Win!";
            yield return new WaitForSeconds(0.4f);
            _winText.text = "";
            yield return new WaitForSeconds(0.4f);
        }

    }

    IEnumerator WavePresentation()
    {
        _waveText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        _waveText.gameObject.SetActive(false);
           
    }
}
