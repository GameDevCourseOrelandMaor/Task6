using System.Collections;
using UnityEngine;


/**
 * This component represents a player that can shoot using left-click and reload using R.
 * The player has limited ammunition.
 */
public class Shooter : MonoBehaviour
{
    private Actions actions;

    [SerializeField] Transform muzzlePos;

    [Tooltip("Particle-effect that is triggered near the weapon mouth when the player shoots")]
    [SerializeField]
    private GameObject muzzleFlash = null;

    [Tooltip("Particle-effect that is triggered where the player's bullet hits")]
    [SerializeField]
    private GameObject bulletHole = null;

    [Tooltip("How many bullets the player initially has")]
    [SerializeField]
    private int startAmmo = 50;

    [Tooltip("How many bullets the player currently has")]
    [SerializeField]
    private int ammo;


    [SerializeField]
    private Transform dest;

    [SerializeField]
    private float speeddest = 20;

    [SerializeField]
    private LayerMask mask;


    private bool _isReloading;

    void Start()
    {
        ammo = startAmmo;
        if (muzzleFlash)
        {
            muzzleFlash.SetActive(false);
        }
    }

    private void Awake()
    {
        actions = GetComponent<Actions>();
    }

    void Update()
    {
        Vector2 screenCentre = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCentre);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, mask))
        {
            dest.position = Vector3.Lerp(dest.position, hitInfo.point, speeddest * Time.deltaTime);
        }

        if (Input.GetMouseButton(0) && !_isReloading)
        {
            if (ammo > 0)
            {
                if (actions)
                {
                    actions.Attack();
                }

                if (hitInfo.collider)
                {
                    GameObject hitMarker = Instantiate(bulletHole, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    Destroy(hitMarker, 1f);
                    if (hitInfo.collider.CompareTag("Enemy"))
                    {
                        hitInfo.collider.GetComponent<DeathHandle>().Elliminate();
                        Debug.Log("Enemy is hit");
                    }
                }


                ammo--;
            }
            else
            {
                Debug.Log("There is no ammo left!");
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && !_isReloading)
        {
            _isReloading = true;
            Reload();
        }
    }

    private void Reload()
    {
        StartCoroutine(ShootingCoolDownRoutine());
    }

    IEnumerator ShootingCoolDownRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        ammo = startAmmo;
        _isReloading = false;
    }

    IEnumerator CoolDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
    }

    public void _addAmmo()
    {
        ammo = startAmmo;
    }
}