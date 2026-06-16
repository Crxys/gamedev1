using UnityEngine;
using UnityEngine.Events;
public class playerHealth : MonoBehaviour
{
    public delegate void playerDeath();
    public static event playerDeath OnDeath;
    private int health = 10;
    private bool alive = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0 && alive)
        {
            OnDeath.Invoke();
            alive = false;
        }
    }
}
