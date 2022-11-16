using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rigidbody;
    [SerializeField] private float moveSpeed = 8;
    [SerializeField] private float swipeSpeed = 8;
    [SerializeField] private float maxXRange = 0.94f;
    [SerializeField] private float minXRange = -0.94f;
    [SerializeField] private LayerMask groundMask;
  
    private Camera camera;
    private Animator animator;
    private Vector3 targetPosition;
    private bool swipe = false;
    public Transform testObj;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        camera = Camera.main;
        animator.SetFloat("BlendSpeed", 1);
    }

  
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            swipe = true;
        }
        if (Input.GetMouseButton(0) && swipe)
        {
            
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                targetPosition = new Vector3(hit.point.x, transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, swipeSpeed * Time.deltaTime);
                Vector3 lookDirection = (targetPosition - transform.position).normalized;
                //transform.rotation =Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection),moveSpeed);
               
            }
            //transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.World);
            ClampXRange();
        }
        if (Input.GetMouseButtonUp(0))
        {
            swipe = false;
        }
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.World);
        ClampXRange();

    }
    private void FixedUpdate()
    {
       // rigidbody.MovePosition(rigidbody.position + targetPosition * moveSpeed * Time.fixedDeltaTime);
    }
    private void ClampXRange()
    {
        if (transform.position.x >= maxXRange)
        {
            Vector3 currentPosition = transform.position;
            currentPosition.x = maxXRange;
            currentPosition.y = transform.position.y; 
            currentPosition.z = transform.position.z;
            transform.position = currentPosition;
        }

        else if(transform.position.x < minXRange)
        {
            Vector3 currentPosition = transform.position;
            currentPosition.x = minXRange;
            currentPosition.y = transform.position.y; ;
            currentPosition.z = transform.position.z;
            transform.position = currentPosition;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
       
    }
    public void PlayPunchAnimation()
    {
       
        animator.SetTrigger("Punch");
    }
    
}
