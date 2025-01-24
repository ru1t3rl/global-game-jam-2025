using UnityEngine;

public class BeeBehavior : MonoBehaviour
{

    [Header("Bee Movement")]    
    
    [SerializeField] private float idleSpeed = 5f;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float stingDistance = 1f;

    [SerializeField] private float idleWanderRadius = 5f;
    [SerializeField] private Vector3 idelTarget;
    [SerializeField] private Transform target;
    [SerializeField] private bool isChasing = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateIdleTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if(isChasing){
            ChaseTarget();
        }else{
            IdleBehavior();
            DetectTarget();
        }
    }

    private void IdleBehavior(){

        transform.position = Vector3.MoveTowards(transform.position, idelTarget, idleSpeed * Time.deltaTime);

        //if close enough to the idle target, generate a new one
        if(Vector3.Distance(transform.position, idelTarget) < 0.5f){
            GenerateIdleTarget();
        }
    }

    /// <summary>
    /// Generate an idle target for the bee to move to.
    /// </summary>
    private void GenerateIdleTarget(){
        Vector3 randomDirection = Random.insideUnitSphere * idleWanderRadius;
        //TODO change the Y position to be more realistic.
        randomDirection.y = 0;
        idelTarget = transform.position + randomDirection;

    }

    private void DetectTarget(){
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var hit in hits)
        {
            if(hit.CompareTag("Target")){
                target = hit.transform;
                isChasing = true;
                break;
            }
        }
    }

    //TODO Add rotation of the bee to face the target
    private void Rotate()
    {
        
    }

    private void ChaseTarget()
    {
        if(target == null)
        {
            //Target Lost, return to Idle
            isChasing = false;
            return;
        }

        //TODO change to work with dotween!!!
        transform.position = Vector3.MoveTowards(transform.position, target.position, chaseSpeed * Time.deltaTime);

        if(CheckDistanceToTarget())
        {
            StingTarget(target);
        }
    }

    private bool CheckDistanceToTarget()
    {
        return Vector3.Distance(transform.position, target.position) <= stingDistance;
    }

    private void StingTarget(Transform target)
    {
        // Perform the stinging action (e.g., animation, damage, etc.)
        Debug.Log("Stung the target!");
        Destroy(target.gameObject);

    }

    private void OnDrawGizmosSelected()
    {
        // Draw the detection radius in the editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Draw the sting distance in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stingDistance);
    }

}
