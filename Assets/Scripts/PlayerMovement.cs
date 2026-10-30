using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float maxForce = 25f;      // максимальная сила выстрела
    public float minDrag = 0.5f;      // минимальное натяжение, чтобы выстрел сработал
    public LineRenderer aimLine;      // линия прицела
    public GamePresenter presenter;
    public AudioClip shootSound;      
    private AudioSource audioSource;
    public AudioClip damageSound;

    private Rigidbody rb;
    private Camera cam;
    private bool aiming = false;
    private Vector3 mousePoint;       // точка на полу, где мышка

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        if (aimLine != null) aimLine.enabled = false;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Стрелять можно только когда шарик почти остановился
        bool canShoot = rb.linearVelocity.magnitude < 5f;

        // Нажали ЛКМ — начали прицеливаться (только если можно стрелять)
        if (canShoot && Input.GetMouseButtonDown(0))
        {
            if (GetMouseOnFloor(out mousePoint))
            {
                aiming = true;
                if (aimLine != null) aimLine.enabled = true;
            }
        }

        // Ведём мышью — обновляем линию
        if (aiming && Input.GetMouseButton(0))
        {
            if (GetMouseOnFloor(out mousePoint))
            {
                DrawLine();
            }
        }

        // Отпустили — стреляем
        if (aiming && Input.GetMouseButtonUp(0))
        {
            aiming = false;
            if (aimLine != null) aimLine.enabled = false;
            Shoot();
        }
    }

    // Луч из камеры в пол, получаем точку под мышкой
    bool GetMouseOnFloor(out Vector3 point)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane floor = new Plane(Vector3.up, Vector3.zero);
        if (floor.Raycast(ray, out float dist))
        {
            point = ray.GetPoint(dist);
            return true;
        }
        point = Vector3.zero;
        return false;
    }

    // Рисуем линию от шарика в сторону полёта
    void DrawLine()
    {
        Vector3 dir = transform.position - mousePoint;  // тянем назад — летим вперёд
        dir.y = 0;
        if (dir.magnitude < 0.1f) return;

        aimLine.positionCount = 2;
        aimLine.SetPosition(0, transform.position);
        aimLine.SetPosition(1, transform.position + dir.normalized * 4f);
    }

    // Сам выстрел
    void Shoot()
    {
        Vector3 dir = transform.position - mousePoint;
        dir.y = 0;

        float strength = dir.magnitude;
        if (strength < minDrag) return;

        dir.Normalize();

        // Сила = насколько сильно натянули, но не больше maxForce
        float force = Mathf.Clamp(strength, 0f, maxForce);
        rb.AddForce(dir * force, ForceMode.Impulse);

        if (shootSound != null)
            audioSource.PlayOneShot(shootSound);
    }

    public void TakeDamage()
    {
        if (damageSound != null && audioSource != null)
            audioSource.PlayOneShot(damageSound, 0.4f);
        presenter.PlayerTakeDamage();
    }
}