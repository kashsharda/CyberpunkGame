using UnityEngine;
public class PatrolEnemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform checkPoint;
    public float distance = 1f;
    public LayerMask layerMask;
    public bool facingLeft = true;
    public bool inRange = false;
    public Transform player;
    public float attackRange = 5f;
    public float retrieveDistance = 2.5f;
    public float chaseSpeed = 4f;
    public Animator animator;

    public Transform attackPoint;
    public float attackRadius = 0.5f;
    public LayerMask attackLayer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            inRange = true;

        }
        else 
        {
            inRange = false;
        }

        if (inRange)
        {
            if (player.position.x > transform.position.x && facingLeft == true)
            {
                transform.eulerAngles = new Vector3(0,-180,0);
                facingLeft = false;
            }
            else if (player.position.x < transform.position.x && facingLeft == false)
            {
                transform.eulerAngles = new Vector3(0,0,0);
                facingLeft = true;
            }
            if (Vector2.Distance(transform.position, player.position) > retrieveDistance)
            {
                animator.SetBool("Attack1", false);
                transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
            }
            else 
            {
                animator.SetBool("Attack1", true);

            }
        }

        else
        {
            // Keeping the character within the patrol range and making it move back and forth only
            transform.Translate(Vector2.left* Time.deltaTime * moveSpeed);

            RaycastHit2D hit = Physics2D.Raycast(checkPoint.position, Vector2.down, distance, layerMask);

            if (hit == false && facingLeft == true)
            {
                transform.eulerAngles = new Vector3(0,-180,0);
                facingLeft = false;
            }

            else if (hit == false && facingLeft == false)
            {
                transform.eulerAngles = new Vector3(0,0,0);
                facingLeft = true;
            }
        }
        
    }

    public void attack ()
    {
        Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);
        if (collInfo)
        {
            if (collInfo.gameObject.GetComponent<movement>() != null)
            {
                collInfo.gameObject.GetComponent<movement>().TakeDamage(1);
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        if (checkPoint == null)
        {
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(checkPoint.position, Vector2.down * distance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);

    }


}
