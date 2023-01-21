using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected int lives;
    public virtual void GetDamage()
    {
        lives--;
        if (lives == 0)
            Invoke("Die", 1f);
    }

    public virtual void Die()
    {
        gameObject.tag = "enemydead";
        LevelController.Instance.EnemiesCount();
        Destroy(this.gameObject);
    }
}
