using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 5f;

    private float direction;
    private bool hit;
    private float lifetime;

    private Animator anim;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (hit) return;

        // Move fireball horizontally only
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        // Lifetime limit to prevent infinite fireballs
        lifetime += Time.deltaTime;
        if (lifetime > 5f)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        boxCollider.enabled = false;
        anim.SetTrigger("explode");

        // Damage enemy if it has Health script
        if (collision.CompareTag("Enemy"))
            collision.GetComponent<Health>()?.TakeDamage(1);
    }

    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);

        hit = false;
        boxCollider.enabled = true;

        // Flip fireball sprite if needed
        float localScaleX = Mathf.Abs(transform.localScale.x) * _direction;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    // Called by animation event at end of "explode" animation
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
