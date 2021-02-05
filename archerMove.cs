using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class archerMove : MonoBehaviour
{
    [SerializeField] float speed = 4f;
    [SerializeField] LayerMask terrain;
    Animator animator;
    private prefabs prefabs;
    Vector3 direction;
    private bool isAiming = false;
    private float canShoot = 50;
    public float charge = 0;
    public float Health = 100;

    private void Awake() => animator = GetComponent<Animator>();

    private void Start()
    {
        prefabs = GameObject.Find("Prefabs").GetComponent<prefabs>();
    }

    private void FixedUpdate()
    {
        canShoot++;

        if(isAiming == true && charge < 100)
        {
            charge = charge + 0.8f;
        }
    }
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, 0f, vertical);

        if (Health > 0)
        {
            aim();
            shoot();
            moving(movement);
        }
        else
            animator.SetBool("dead", true);
         
        float moveX = Vector3.Dot(movement.normalized, transform.right);
        float moveZ = Vector3.Dot(movement.normalized, transform.forward);

        animator.SetFloat("moveX", moveX, 0.1f, Time.deltaTime);
        animator.SetFloat("moveZ", moveZ, 0.1f, Time.deltaTime);
        animator.SetBool("aiming", isAiming);
    }

    void aim()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, terrain))
        {
            this.transform.LookAt(hit.point);
            direction = new Vector3(hit.point.x, hit.point.y + 100f, hit.point.z);
        }
    }

    void shoot()
    {
        if (Input.GetMouseButtonDown(0) && canShoot > 50)
        {
            canShoot = 0;
            charge = 0;
            isAiming = true;
            animator.SetTrigger("draw");
            Instantiate(prefabs.arrow, transform.position + direction.normalized, transform.rotation);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isAiming = false;
            animator.SetTrigger("fire");  
        }

        if (isAiming == false && canShoot < 50 || isAiming == true)
            speed = 1f;
        else
            speed = 4f;
    }

    void moving(Vector3 movement)
    {
        if (movement.magnitude > 0)
        {
            transform.Translate(movement.normalized * speed * Time.deltaTime, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "zombieHand")
            Health = Health - 10;
    }

}
