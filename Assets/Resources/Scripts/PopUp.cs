using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PopUp : MonoBehaviour
{
    [HideInInspector]
    public PopUpGenerator generator;

    public UnityEvent OnOpen;
    public UnityEvent OnClose;
    private Animator animator;

    protected float displayTime = 0;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        OnOpen.Invoke();
        displayTime = 0;
        //GameManager.Instance.ignoreGameObjRaycast = true;

    }

    protected virtual void Update()
    {
        displayTime += Time.deltaTime;
    }

    public virtual void Close()
    {
        OnClose.Invoke();
        //GameManager.Instance.ignoreGameObjRaycast = false;


        if (generator != null)
        {
            generator.ClosePopup();
        }

        if (animator != null)
        {
            animator.Play("Close");
            StartCoroutine(DestroyPopup());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyPopup()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

}
