using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    #region Inspector Settings

    [Header("General Settings")]
    [SerializeField] private string enemyName = "Enemy";

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private string health;

    #endregion

    [Header("UI References")]
    [SerializeField] private TMP_Text healthText;

    private void Awake()
    {
        health = CalculateHealth();
        UpdateHealthDisplay();
        Debug.Log($"{enemyName} initialized with health: {health}");
    }

    private string CalculateHealth()
    {
        int currentLevel = GameManager.instance.GetCurrentLevel();
        float numEnemy = GameManager.instance.numOfEnemy;
        int n;
        string result = "";

        if (currentLevel == 1)
        {
            result = ((int)Random.Range(1 + numEnemy / 1.5f, numEnemy * 1.5f + 4)).ToString();
        }
        else if (currentLevel == 2)
        {
            n = Random.Range(1, 100);
            if (n < 25)
            {
                result = ((int)Random.Range(4 + numEnemy / 1.5f, numEnemy * 1.6f + 7)).ToString();
            }
            else
            {
                result = ((int)Random.Range(-numEnemy * 1.5f - 4, -numEnemy / 1.5f - 2)).ToString();
            }
        }
        else if (currentLevel == 3)
        {
            int upper = (int)Random.Range(4 + numEnemy * 3, numEnemy * 4 + 6);
            int lower = (int)Random.Range(2, numEnemy / 2 + 3);
            n = Random.Range(1, 100);

            if (n < 20)
            {
                result = ((int)(upper * 0.7f)).ToString();
            }
            else
            {
                while (upper % lower == 0 || IsPrime(lower))
                {
                    lower++;
                }
                result = upper.ToString() + "/" + lower.ToString();
            }
            if (n > 10 && n < 60)
            {
                result = "-" + result;
            }
        }
        else if (currentLevel == 4)
        {
            int upper = (int)Random.Range(6 + numEnemy * 7, numEnemy * 10 + 8);
            int lower = (int)Random.Range(2 + numEnemy, numEnemy * 3 + 6);
            n = Random.Range(1, 100);

            if (n < 40)
            {
                result = (upper * 3).ToString();
            }
            else
            {
                while (upper % lower == 0 || IsPrime(lower))
                {
                    lower++;
                }
                result = upper.ToString() + "/" + lower.ToString();
            }
            if (n > 20 && n < 70)
            {
                result = "-" + result;
            }
        }
        else
        {
            result = maxHealth.ToString();
        }

        return result;
    }

    private bool IsPrime(int number)
    {
        if (number <= 1)
            return false;
        if (number == 2)
            return true;
        if (number % 2 == 0)
            return false;

        int boundary = (int)Mathf.Floor(Mathf.Sqrt(number));
        for (int i = 3; i <= boundary; i += 2)
        {
            if (number % i == 0)
                return false;
        }
        return true;
    }

    private void UpdateHealthDisplay()
    {
        if (healthText == null)
        {
            healthText = GetComponentInChildren<TMP_Text>();
        }

        if (healthText != null)
        {
            healthText.text = health;
        }
        else
        {
            Debug.LogWarning($"{enemyName} is missing a TMP_Text component to display health.");
        }
    }
}
