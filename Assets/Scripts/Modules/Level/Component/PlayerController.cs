using UnityEngine;

///<summary>
///选关界面人物简单控制脚本
///</summary>
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2.5f;
    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        GameApp.CameraManager.SetPosition(this.transform.position);
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if (h == 0)
        {
            animator.Play("idle");
        }
        else
        {
            if (h * transform.localScale.x < 0)
            {
                Flip();
            }
            Vector3 pos = transform.position + Vector3.right * h * moveSpeed * Time.deltaTime;
            pos.x = Mathf.Clamp(pos.x, -32, 24);
            transform.position = pos;
            animator.Play("move");

            GameApp.CameraManager.SetPosition(this.transform.position);
        }
    }

    //转向
    public void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
