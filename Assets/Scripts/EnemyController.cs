using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Vector2 velocity;
    public Rigidbody2D rb;
    public GameObject liveSprite = default;
    public GameObject deathSprite = default;
    public Transform minTrans, maxTrans;
    public float speed = 2f;
    public float disCheck = .75f;

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.right * speed, disCheck, LayerMask.GetMask("Obtacles"));

        if (hit.collider != null )
            speed = -speed;
        if (speed > 0)
            transform.eulerAngles = Vector2.zero;
        else if (speed < 0)
            transform.eulerAngles = Vector2.up * 180;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(rb.position, rb.position + new Vector2(disCheck * speed / Mathf.Abs(speed),0));
    }
    private void FixedUpdate()
    {
        Vector2 pos = rb.position;
        if (pos.x == minTrans.position.x || pos.x == maxTrans.position.x) speed = -speed;
        pos.x += speed * Time.fixedDeltaTime;
        pos.x = Mathf.Clamp(pos.x, minTrans.position.x, maxTrans.position.x);
        rb.MovePosition(pos);
    }

    public void ShowDeathAnim()
    {
        StartCoroutine(DeathControl());
    }

    private IEnumerator DeathControl()
    {
        liveSprite.SetActive(false);
        deathSprite.SetActive(true);
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(.2f);
        gameObject.transform.parent.gameObject.SetActive(false);
        liveSprite.SetActive(true);
        deathSprite.SetActive(false);
    }
}
