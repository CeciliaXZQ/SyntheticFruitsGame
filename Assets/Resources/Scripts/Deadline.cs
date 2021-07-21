using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deadline : MonoBehaviour
{
    private Collider2D lineCollider;
    private Coroutine disableColliderCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        lineCollider = GetComponent<Collider2D>();
    }

    public void DisableForAWhile(float duration)
    {
        if (disableColliderCoroutine != null)
        {
            StopCoroutine(disableColliderCoroutine);
        }
        disableColliderCoroutine = StartCoroutine(DisableForAWhileIEn(duration));
    }

    IEnumerator DisableForAWhileIEn(float duration)
    {
        lineCollider.enabled = false;
        yield return new WaitForSeconds(duration);
        lineCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var ball = collision.GetComponent<Ball>();
        if (ball != null && GameManager.Instance.ThisGameState.Equals(GameManager.GameState.InGame))
        {
            Debug.Log("GameOver");
            GameManager.Instance.ThisGameState = GameManager.GameState.GameOver;
        }
    }

}
