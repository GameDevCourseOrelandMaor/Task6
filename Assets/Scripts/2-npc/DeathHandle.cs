using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandle : MonoBehaviour
{
    private Actions actions;
    private bool _isAlive;

    [SerializeField]private float _timeToDestroy = 5f;

    void Start()
    {
        actions = GetComponent<Actions>();
        _isAlive = true;
        actions.Aiming(); 
    }

   public void Elliminate(){

        if(_isAlive){
            _isAlive = false;
            actions.Death();
            Destroy(gameObject, _timeToDestroy);
        }

    }
}
