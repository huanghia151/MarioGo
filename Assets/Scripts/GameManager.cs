using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public int lives { get; private set; }
    public int coins { get; private set; }
    public void AddCoin()
    {
        coins++;

        if (coins == 100)
        {
            coins = 0;
            //AddLife();
        }
    }

    //public void AddLife()
    //{
    //    lives++;
    //}
    public void NewGame()
    {
        lives = 3;
        coins = 0;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(0);
        //SceneManager.LoadScene("Game");
    }
    public void GameOver()
    {
        // TODO: show game over screen
        
        NewGame();
    }
    public void ResetLevel()
    {
        lives--;

        if (lives < 0)
        {
            Invoke("GameOver", 3.0f);
            //GameOver();
        }
    }
}
