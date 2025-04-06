using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TitleBoard board;
    public CanvasGroup gameOver;
    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI bestScoreText;

    private int score;
    private void Start()
    {
        NewGame();
    }


 
    public void NewGame()
    {
        SetScore(0);
        bestScoreText.text = LoadBestScore().ToString();
        gameOver.alpha = 0f;
        gameOver.interactable = false;
        board.ClearBoard();
        board.CreateTitle();
        board.CreateTitle();
        board.enabled = true;
    }

    public void GameOver()
    {
        board.enabled = false;
        gameOver.interactable = true;
        StartCoroutine(Fade(gameOver, 1f, 1f));
    }

    public void Quit()
    {
        Application.Quit();
    }
    private IEnumerator Fade(CanvasGroup canvasGroup,float to,float delay)
    {
        yield return new WaitForSeconds(delay);

        float elapsedTime = 0f;
        float duration = 0.5f;
        float from = canvasGroup.alpha;

        while (elapsedTime < duration)
        {
           
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = to;
    }

    public void IncreaseScore(int point)
    {
        SetScore(score + point);
    }

    public void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();
        SaveBestScore();
    }

    public void SaveBestScore()
    {
        int bestscore = LoadBestScore();
        if(score > bestscore)
        {
            PlayerPrefs.SetInt("bestscore", score);
        }
    }

    private int LoadBestScore()
    {
        return PlayerPrefs.GetInt("bestscore", 0);
    }
}
