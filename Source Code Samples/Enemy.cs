using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public Transform target;
    private Rigidbody2D rb;

    public Transform firepoint2;
    public GameObject enemybulletPrefab;

    public float bulletSpeed = 15f;

    public float fireRate = 0.5F;
    private float nextFire = 0.0F;

    public static float health;

    public GameObject EntityRecoil;
    public float RecoilForce = 0.75f;

    //flash when hit
    private Material matRed;
    private Material matDefault;
    SpriteRenderer spr;
    public GameObject DamageSprite;

    //movement
    public float accelerationTime = 2f;
    public float maxSpeed = 5f;
    private Vector2 movement;
    private float timeLeft;

    void Start()
    {
        health = 1.0f;
        rb = this.GetComponent<Rigidbody2D>();

        spr = DamageSprite.GetComponent<SpriteRenderer>();
        matRed = Resources.Load("RedFlash", typeof(Material)) as Material;
        matDefault = spr.material;
    }

    void Update()
    {
        //look at player
        Vector3 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle;

        //shoot
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Shoot();
        }

        //movement
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            timeLeft += accelerationTime;
        }
    }

    void Shoot()
    {
        GameObject Enemy_Bullet = Instantiate(enemybulletPrefab, firepoint2.position, firepoint2.rotation);
        Rigidbody2D rb = Enemy_Bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firepoint2.up * bulletSpeed, ForceMode2D.Impulse);
        Rigidbody2D rbp = EntityRecoil.GetComponent<Rigidbody2D>();
        rbp.AddForce(-firepoint2.up * RecoilForce, ForceMode2D.Impulse);
    }

    public void Damage(float damage)
    {
        spr.material = matRed;      
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        else Invoke("ResetMaterial", .075f);
    }

    void Die()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("Level1");
    }

    //flash when hit
    void ResetMaterial()
    {
        spr.material = matDefault;
    }

    //movement
    void FixedUpdate()
    {
        rb.AddForce(movement * maxSpeed);
    }
}
