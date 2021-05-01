using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _scoreText;
    [SerializeField]
    private Image _livesDisplay;
    [SerializeField]
    private TextMeshProUGUI _gameOverText;
    [SerializeField]
    private TextMeshProUGUI _restartText;

    [SerializeField]
    private GameObject playerPrefab;

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
    }

    void RestartGame()
    {
        _isGameOver = false;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        _livesDisplay.sprite = _livesSprites[3];
        _currentScore = 0;
        _scoreText.text = "Score: " + _currentScore;
        _spawnManager.GameRestarted();
    }

    public void UpdateScore()
    {
        _currentScore += 10;
        _scoreText.text = "Score: " + _currentScore;
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
}
