using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;          // скорость движения к шарику
    public float killSpeed = 4f;      // скорость шарика, при которой кубик ломается
    public AudioClip killSound;
    public GameObject killEffectPrefab;

    private Transform player;
    private Rigidbody playerRb;

    void Start()
    {
        // Находим шарик по тегу
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerRb = playerObj.GetComponent<Rigidbody>();
        }
    }

    void Update()
    {
        // Каждый кадр двигаемся к шарику
        if (player == null) return;

        Vector3 dir = player.position - transform.position;
        dir.y = 0;
        if (dir.magnitude > 0.1f)
        {
            transform.position += dir.normalized * speed * Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Кубик столкнулся с шариком
        if (collision.gameObject.CompareTag("Player"))
        {
            float ballSpeed = collision.relativeVelocity.magnitude;

            if (ballSpeed >= killSpeed)
            {
                // Шарик летел быстро — кубик разбивается
                Die(collision);
            }
            else
            {
                // Шарик медленный — игрок получает урон
                collision.gameObject.SendMessage("TakeDamage", SendMessageOptions.DontRequireReceiver);
                Destroy(gameObject);   // кубик тоже умирает (иначе будет вечно стоять на шарике)
            }
        }
    }

    void Die(Collision collision)
    {
        // Прибавка к скорости шарика
        Rigidbody ballRb = collision.gameObject.GetComponent<Rigidbody>();
        if (ballRb != null)
        {
            // Добавляем импульс в направлении текущего полёта шарика
            Vector3 boostDir = ballRb.linearVelocity.normalized;
            ballRb.AddForce(boostDir * 10f, ForceMode.Impulse);
            // В Enemy.Die() после boost:
            GameObject playerObj = collision.gameObject;
            PlayerMovement ps = playerObj.GetComponent<PlayerMovement>();
            if (ps != null && ps.presenter != null)
            {
                ps.presenter.AddScore(1);
            }
            if (killEffectPrefab != null)
            {
                GameObject effect = Instantiate(killEffectPrefab, transform.position, Quaternion.identity);
                effect.GetComponent<ParticleSystem>().Play();
                Destroy(effect, 1f);   // удалить через 1 сек, чтобы не копились
            }

            Destroy(gameObject);
        }

        
        if (SoundManager.Instance != null && killSound != null)
            SoundManager.Instance.PlaySound(killSound, 0.8f);

    Destroy(gameObject);
    }
}