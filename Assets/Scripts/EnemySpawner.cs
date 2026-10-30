using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float arenaHalfSize = 10f; // половина размера арены (пол 20x20)
    public float startInterval = 2f;  // интервал спавна в начале
    public float minInterval = 0.5f;  // минимальный интервал (предел сложности)
    public float speedUpRate = 0.02f; // насколько интервал сокращается каждую секунду

    private float timer = 0f;
    private float currentInterval;
    private int enemiesAlive = 0;
    private int maxEnemies = 30;
    public float minDistanceFromPlayer = 4f;  // минимальное расстояние от шарика до точки спавна
    public int maxSpawnAttempts = 10;         // сколько раз пробовать найти хорошую точку

    void Start()
    {
        currentInterval = startInterval;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentInterval && enemiesAlive < maxEnemies)
        {
            SpawnEnemy();
            timer = 0f;
            // Уменьшаем интервал со временем
            currentInterval = Mathf.Max(minInterval, currentInterval - speedUpRate * Time.deltaTime);
        }
    }

    void SpawnEnemy()
    {
        // Ищем точку, которая далеко от шарика
        Vector3 spawnPos = Vector3.zero;
        bool found = false;

        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            spawnPos = GetRandomEdgePoint();

            // Если шарика нет на сцене — спавним куда угодно
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj == null)
            {
                found = true;
                break;
            }

            float dist = Vector3.Distance(spawnPos, playerObj.transform.position);
            if (dist >= minDistanceFromPlayer)
            {
                found = true;
                break;
            }
        }

        // Если за 10 попыток не нашли далёкую точку — пропускаем спавн
        if (!found) return;

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        enemiesAlive = GameObject.FindObjectsByType<Enemy>().Length;
    }

    void CheckEnemyDead()
    {
        // Грубая проверка: пересчитываем живых раз в 0.1 сек
        enemiesAlive = GameObject.FindObjectsByType<Enemy>().Length;
    }

    Vector3 GetRandomEdgePoint()
    {
        // Выбираем случайную сторону (0=север, 1=юг, 2=запад, 3=восток)
        int side = Random.Range(0, 4);
        float along = Random.Range(-arenaHalfSize + 1f, arenaHalfSize - 1f);
        float edge = arenaHalfSize - 1f;

        switch (side)
        {
            case 0: return new Vector3(along, 0.5f, edge);   // север
            case 1: return new Vector3(along, 0.5f, -edge);  // юг
            case 2: return new Vector3(-edge, 0.5f, along);  // запад
            default: return new Vector3(edge, 0.5f, along);  // восток
        }
    }
}