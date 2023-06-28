using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    //public Player play;
    public enum Type
    {
        Coin,
        ExtraLife,
        MagicMushroom,
    }

    public Type type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Collect(other.gameObject);
        }
    }

    private void Collect(GameObject player)
    {
        switch (type)
        {
            case Type.Coin:
                GameManager.Instance.AddCoin();
                break;

            //case Type.ExtraLife:
            //    GameManager.Instance.AddLife();
            //    break;

            case Type.MagicMushroom:
                player.GetComponent<Player>().Grow();
                break;
        }
        //Debug.LogError("Destroy");
        Destroy(gameObject);
    }
}
