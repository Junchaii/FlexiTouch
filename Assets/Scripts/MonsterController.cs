using System.Collections;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public float Speed = 5.0f;
    private bool Hit = false;
    private bool Moving = true;
    private Rigidbody rigidBody;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        rigidBody = this.GetComponent<Rigidbody>();
        StartCoroutine(MoveAndPause());
    }

    // Update is called once per frame
    void Update()
    {
        if (Moving && !Hit)
        {
            transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        }
    }

    private IEnumerator MoveAndPause()
    {
        while (true)
        {
            // Move for 0.3 seconds
            Moving = true;
            yield return new WaitForSeconds(1f);
            
            // Pause for 0.1 seconds
            Moving = false;
            yield return new WaitForSeconds(3f);
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        Hit = true;
    }
}
