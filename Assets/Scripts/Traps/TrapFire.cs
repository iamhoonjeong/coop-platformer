using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TrapFire : MonoBehaviour
{
    [SerializeField] float offDuration;
    [SerializeField] TrapFireButton fireButton;

    Animator anim;
    CapsuleCollider2D fireCollider;
    bool isActive;

    void Awake()
    {

        anim = GetComponent<Animator>();
        fireCollider = GetComponent<CapsuleCollider2D>();
    }

    void Start()
    {
        if (!fireButton)
        {
            print("you don't have fire button " + gameObject.name + "!");
        }

        SetFire(true);
    }

    public void SwitchOffFire()
    {
        if (!isActive) return;

        StartCoroutine(FireCoroutine());
    }

    IEnumerator FireCoroutine()
    {
        SetFire(false);

        yield return new WaitForSeconds(offDuration);

        SetFire(true);
    }

    void SetFire(bool active)
    {
        anim.SetBool("active", active);
        fireCollider.enabled = active;
        isActive = active;
    }
}
