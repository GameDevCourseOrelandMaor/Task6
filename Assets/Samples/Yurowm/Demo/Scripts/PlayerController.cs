using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{

    public Transform rightGunBone;
    public Transform leftGunBone;
    public Arsenal[] arsenal;

    private Animator animator;
    [SerializeField] private InputAction changeWeapon = new InputAction(type: InputActionType.Button);
    private int arsenalIndex = 0;

    private void OnEnable()
    {
        changeWeapon.Enable();
    }

    private void OnDisable()
    {
        changeWeapon.Disable();
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (arsenal.Length > 0)
            SetArsenal(arsenal[arsenalIndex].name);
    }

    public void FireCurrentWeapon()
    {
        var weapon = arsenal[arsenalIndex];
        if (weapon.name == "Rifle")
        {
            ShowFlash gun = weapon.rightGun.GetComponentInChildren<ShowFlash>();
            if (gun)
            {
                gun.Fire();
            }
        }

    }

    public void SetArsenal(string name)
    {
        foreach (Arsenal hand in arsenal)
        {
            if (hand.name == name)
            {
                if (rightGunBone.childCount > 0)
                    Destroy(rightGunBone.GetChild(0).gameObject);
                if (leftGunBone.childCount > 0)
                    Destroy(leftGunBone.GetChild(0).gameObject);
                if (hand.rightGun != null)
                {
                    GameObject newRightGun = (GameObject)Instantiate(hand.rightGun);
                    newRightGun.transform.parent = rightGunBone;
                    newRightGun.transform.localPosition = Vector3.zero;
                    newRightGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
                }

                if (hand.leftGun != null)
                {
                    GameObject newLeftGun = (GameObject)Instantiate(hand.leftGun);
                    newLeftGun.transform.parent = leftGunBone;
                    newLeftGun.transform.localPosition = Vector3.zero;
                    newLeftGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
                }

                animator.runtimeAnimatorController = hand.controller;
                return;
            }
        }
    }



    private void Update()
    {
        if (changeWeapon.WasPerformedThisFrame())
        {
            arsenalIndex = (arsenalIndex + 1) % arsenal.Length;
            SetArsenal(arsenal[arsenalIndex].name);
        }
    }

    [System.Serializable]
    public struct Arsenal
    {
        public string name;
        public GameObject rightGun;
        public GameObject leftGun;
        public RuntimeAnimatorController controller;
    }
}