using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour
{
    public float damage = 0.35f;

    public int knockbackForce;

    void OnTriggerEnter2D(Collider2D hit)
    {
        /*
        Enemy enemy = hit.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.Damage(damage);
        }
        Player player = hit.GetComponent<Player>();
        if (player != null)
        {
            player.Damage(damage);
        }
        */

        if (hit.gameObject.CompareTag("tanks"))
        {
            Rigidbody2D tanks = hit.GetComponent<Rigidbody2D>();
            if(tanks != null)
            {
                Vector2 difference = tanks.transform.position - transform.position;
                difference = difference.normalized * knockbackForce;
                tanks.AddForce(difference, ForceMode2D.Impulse);
            }
        }
        if (!(hit.gameObject.CompareTag("trigger")))
            Destroy(gameObject);
    }
}
