using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 
public class KnocKBack : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float strength = 16, delay = 0.15f;

    public UnityEvent OnBegin, OnDone;

    public void KnockBacK(GameObject sender)
    {
      //  StopAllCoroutines();
        OnBegin?.Invoke();

       /* Rigidbody2D senderRb = sender.GetComponent<Rigidbody2D>();
        Vector2 direction;

        if (senderRb != null)
        {
            direction = senderRb.velocity.normalized;
        }
        else
        {
            direction = (transform.position - sender.transform.position).normalized;
        }

       // Debug.Log("Dirección del Knockback: " + direction);

        rb.AddForce(direction * strength, ForceMode2D.Impulse);*/
        StartCoroutine(Reset());
    }
    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector3.zero;
        OnDone?.Invoke();
    }
}
