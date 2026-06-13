
using UnityEngine;

public class EnemyPlant : Enemy
{
    [Header("Plant details")]
    [SerializeField] EnemyBullet bulletPrefab;
    [SerializeField] Transform gunPoint;
    [SerializeField] float bulletSpeed = 7f;
    [SerializeField] float attackCooldown = 1.5f;
    float lastTimeAttacked;

    protected override void Update()
    {
        base.Update();

        bool canAttack = Time.time > lastTimeAttacked + attackCooldown;

        if (isPlayerDetected && canAttack) Attack();
    }

    void Attack()
    {
        lastTimeAttacked = Time.time;
        anim.SetTrigger("attack");
    }

    void CreateBullet()
    {
        EnemyBullet newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.identity);
        Vector2 bulletVelocity = new Vector2(facingDir * bulletSpeed, 0);
        newBullet.SetVelocity(bulletVelocity);

        Destroy(newBullet.gameObject, 10);
    }

    protected override void HandleAnimator()
    {
        // Keep it empty, unless need to update parameters
    }
}

