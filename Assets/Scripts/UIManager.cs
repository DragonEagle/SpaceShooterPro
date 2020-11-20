using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _shieldsText;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Player _player;
    [SerializeField]
    private Sprite[] _livesSprites;
    private bool _gameOver = false;
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + _player.Score;
        _shieldsText.text = "Shields: " + _player.Shields;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _gameOver = false;
        _gameManager = FindObjectOfType<GameManager>();
        if (!_gameManager)
        {
            Debug.LogError("Game Manager not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_scoreText)
        {
            _scoreText.text = "Score: " + _player.Score;
        }
        if (_shieldsText)
        {
            _shieldsText.text = "Shields: " + _player.Shields;
        }
        if (_livesImage)
        {
            _livesImage.sprite = _livesSprites[_player.Lives];
        }
        if (_player.Lives == 0) 
        {
            if (!_gameOver)
            {
                _gameOver = true;
                _gameOverText.gameObject.SetActive(true);
                _restartText.gameObject.SetActive(true);
                if (_gameManager)
                {
                    _gameManager.GameOver();
                }
                StartCoroutine(FlickerGameOver());
            }
        }
    }
    IEnumerator FlickerGameOver()
    {
        while (_gameOver)
        {
            yield return new WaitForSeconds(0.1f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            _gameOverText.gameObject.SetActive(true);
        }
    }
}
