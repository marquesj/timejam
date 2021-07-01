using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    public InputGenerator inputGenerator;
    
    public float shootCooldown = 0.2f;
    private Shooter weapon;
    private GameObject arm;
    private bool canShoot = true;
    private void Awake() {
        weapon = GetComponentInChildren<Shooter>();
        if(weapon == null)
        {
            Debug.Log("No weapon found by shoot controller");
            this.enabled = false;
            return;
        }
        if(weapon.transform.parent == null)
        {
            Debug.Log("No arm found by shoot controller");
            this.enabled = false;
            return;
        }
        arm = weapon.transform.parent.gameObject;

            
        inputGenerator.ShootEvent += Shoot;
        inputGenerator.ChangeDirVerticalEvent += BufferMovement;
    }    
    private void Shoot()
    {
        if(canShoot)
        {
            weapon.Shoot();
            canShoot = false;
            Invoke("RestoreShoot", shootCooldown);
        }

    }
    private void BufferMovement(float dir)
    {
        if(dir > 0)
        {
            arm.transform.rotation = Quaternion.Euler(0,0,90);
        }
        else if(dir < 0)
        {
            arm.transform.rotation = Quaternion.Euler(0,0,-90);
        }
        else
        {
            arm.transform.rotation = Quaternion.Euler(0,0,0);
        }
    }

    private void RestoreShoot()
    {
        canShoot = true;
    }
}
