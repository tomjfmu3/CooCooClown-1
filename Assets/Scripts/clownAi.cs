
using UnityEngine;
using UnityEngine.AI;

public class clownAi : MonoBehaviour
{
    public NavMeshAgent agent;


     [Header("Audio")]
    AudioSource soundFX;
    public AudioClip hurt;

    public bool dead = false;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;
    Rigidbody rb;

    public float health;

    	public GameObject explosionPrefab;


    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    public Transform attackPoint;
    public Transform bloodPoint;
    public Transform deathBloodPoint;


        


    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    void Start(){
        soundFX = GetComponent<AudioSource>(); 
    }

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
        
                gameObject.GetComponent<NavMeshAgent>().speed = 3.5f;
       if(!dead){ gameObject.GetComponent<Animator>().Play("walk");}

        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
       if(!dead){ gameObject.GetComponent<Animator>().Play("run");}
        gameObject.GetComponent<NavMeshAgent>().speed = 30f;
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);



        if (!alreadyAttacked)
        {
            //Attack code here
       if(!dead){ gameObject.GetComponent<Animator>().Play("attack");}
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    public void animationAttack(){
         rb = Instantiate(projectile, attackPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
         rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * -8f, ForceMode.Impulse);
    }

    public void animationDeath(){
                Instantiate (explosionPrefab, deathBloodPoint.position, transform.rotation);
			Destroy (gameObject);

    }

    // when collided with another gameObject
	void OnCollisionExit (Collision newCollision)
	{

		// only do stuff if hit by a projectile
		if (newCollision.gameObject.tag == "Projectile") {
			if(ControllerH.diff == 0) {health -= 15;}
            else if(ControllerH.diff == 1) {health -= 10;}
            else if(ControllerH.diff == 2) {health -= 5;}
                        


            Debug.Log(health);
            			Destroy (newCollision.gameObject);
                                        // destroy the projectile

                         Instantiate (explosionPrefab, bloodPoint.position, transform.rotation);


            if (health <= 0){
                soundFX.PlayOneShot(hurt, 1);
                dead = true;

                gameObject.GetComponent<Animator>().StopPlayback();
            //foreach(AnimationClip anim in gameObject.GetComponent<Animator>().runtimeAnimatorController.animationClips){           
           // }

			// die
            gameObject.GetComponent<Animator>().enabled = false;
            gameObject.GetComponent<Animator>().enabled = true;
            gameObject.GetComponent<Animator>().Play("death");
            ControllerH.killCount += 1;
            ControllerH.score += 20;


            }
				
			
		}
	}
}
