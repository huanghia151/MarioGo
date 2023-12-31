using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysic : MonoBehaviour
{
    public Player player;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector2 contactPoint = collision.GetContact(0).point;
            if (Mathf.Abs(Vector2.Angle(contactPoint - GetComponent<Rigidbody2D>().position, Vector2.down)) < 25f)
                collision.transform.GetComponent<EnemyController>().ShowDeathAnim();
            else

                player.Hit();

        }
    }
}
