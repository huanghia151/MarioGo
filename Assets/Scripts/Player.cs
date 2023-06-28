using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public SpriteRenderer smallRenderer;
    public SpriteRenderer bigRenderer;
    private SpriteRenderer activeRenderer;

    public Animator smallAnimator;
    public Animator bigAnimator;
    public AnimationController animsmall;
    public AnimationController animbig;
    public DeathAnimation deathAnimation;
    public CapsuleCollider2D capsule;
    public CapsuleCollider2D capsuleCollider { get; private set; }
//public DeathAnimation deathAnimation { get; private set; }

    public bool big => bigRenderer.enabled;
    public bool dead => deathAnimation.enabled;

    private void Awake()
    {     
        activeRenderer = smallRenderer;
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Enemy"))
    //    {
    //        Hit();
    //    }
    //}
    public void Hit()
    {
        if (!dead)
        {
            if (big)
            {
                Shrink();
            }
            else
            {
                Death();
            }
        }
    }

    public void Death()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        deathAnimation.enabled = true;

        GameManager.Instance.ResetLevel();
    }

    public void Grow()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = true;
        smallAnimator.enabled = false;
        bigAnimator.enabled = true;
        animbig.enabled = true;
        animsmall.enabled = false;
        activeRenderer = bigRenderer;
        //Debug.LogError("Grow");
        //capsuleCollider.size = new Vector2(1f, 2f);
        //capsuleCollider.offset = new Vector2(0f, 0.5f);

        StartCoroutine(ScaleAnimation());
    }

    public void Shrink()
    {
        smallRenderer.enabled = true;
        bigRenderer.enabled = false;
        smallAnimator.enabled = true;
        bigAnimator.enabled = false;
        animbig.enabled = false;
        animsmall.enabled = true;
        activeRenderer = smallRenderer;

        //capsuleCollider.size = new Vector2(1f, 1f);
        //capsuleCollider.offset = new Vector2(0f, 0f);

        StartCoroutine(ScaleAnimation());
    }

    private IEnumerator ScaleAnimation()
    {
        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0)
            {
                smallRenderer.enabled = !smallRenderer.enabled;
                bigRenderer.enabled = !smallRenderer.enabled;
            }

            yield return null;
        }

        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        activeRenderer.enabled = true;
    }

}
