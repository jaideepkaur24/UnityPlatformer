using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs; // Object pool
    [SerializeField] private AudioClip fireballSound;

    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        // Check for input, cooldown, player ability, and that the game isn’t paused
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement.canAttack() && Time.timeScale > 0)
        {
            Attack();
        }

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        // Play sound + animation
        SoundManager.instance.PlaySound(fireballSound);
        anim.SetTrigger("attack");
        cooldownTimer = 0;

        // Grab available fireball and launch
        int index = FindFireball();
        fireballs[index].transform.position = firePoint.position;
        fireballs[index].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private int FindFireball()
    {
        // Look for inactive fireball in pool
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0; // fallback if all are active
    }
}
