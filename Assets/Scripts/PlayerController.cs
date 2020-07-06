using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private PlayerController() { }
    public static PlayerController instance = null;
    public Rigidbody2D theRB;
    public Animator gunAnim;
    public Animator anim;
    public GameObject bulletImpact;
    public Camera viewCam;
    public GameObject deadScreen;
    public Text helthText, ammoText;
    public AudioClip fireAudio;
    public AudioClip gunCockAudio;
    public int currentAmmo = 20;

    private float shootDelay = 0.5f;
    private float mouseSensitivity = 1f;
    private float moveSpeed = 5f;
    private int maxHealth = 100;
    private Vector2 moveInput;
    private Vector2 mouseInput;
    private int currentHealth;
    private bool hasDied;
    private bool canShoot;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        currentHealth = maxHealth;
        canShoot = true;
        helthText.text = currentHealth.ToString() + "%";
        ammoText.text = currentAmmo.ToString();
    }

    private void Update()
    {
        if (!hasDied)
        {
            // player movement
            moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            Vector3 moveHorizontal = transform.up * -moveInput.x;
            Vector3 moveVertical = transform.right * moveInput.y;

            theRB.velocity = (moveHorizontal + moveVertical) * moveSpeed;

            // player view control
            mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y,
                transform.rotation.eulerAngles.z - mouseInput.x);

            viewCam.transform.localRotation = Quaternion.Euler(viewCam.transform.localRotation.eulerAngles +
                new Vector3(0f, mouseInput.y, 0f));

            // shooting
            if (Input.GetMouseButtonDown(0))
            {
                if (currentAmmo > 0 && canShoot)
                {
                    AudioMaster.instance.PlaySFX(fireAudio);
                    canShoot = false;
                    Ray ray = viewCam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
                    RaycastHit hit;
                    StartCoroutine(CountShootDelay());
                    if (Physics.Raycast(ray, out hit))
                    {
                        //Debug.Log("HIT : " + hit.transform.name);
                        Instantiate(bulletImpact, hit.point + new Vector3(0.1f,0,0), transform.rotation);

                        if (hit.transform.tag == "Enemy")
                        {
                            hit.transform.parent.GetComponent<EnemyController>().TakeDamage();
                        }
                    }
                    else
                    {
                        //Debug.Log("NOT HIT");
                    }
                    currentAmmo--;
                    gunAnim.SetTrigger("Shoot");
                    UpdateAmmoUI();
                }
            }

            if (moveInput != Vector2.zero)
            {
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }
        }
    }

    private IEnumerator CountShootDelay()
    {
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            deadScreen.SetActive(true);
            hasDied = true;
            currentHealth = 0;
        }

        helthText.text = currentHealth.ToString() + "%";
    }

    public void AddHealth(int healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        helthText.text = currentHealth.ToString() + "%";
    }

    public void UpdateAmmoUI()
    {
        ammoText.text = currentAmmo.ToString();
    }
}
