using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField] private int score;
    private int enemiesOnScene;

    public static LevelController Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    public virtual void EnemiesCount()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        enemiesOnScene = enemies.Length;
        Debug.Log(enemiesOnScene);
        if (enemiesOnScene <= 0)
            Invoke("goToMenu", 2f);
    }

    private void goToMenu()
    {
        if (SceneManager.GetActiveScene().buildIndex != 2)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
            SceneManager.LoadScene("Menu");
    }
}
