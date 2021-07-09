using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ShootController : MonoBehaviour
{
    [HideInInspector]public event UnityAction ShootEvent;
    [HideInInspector]public event UnityAction ShootBounceEvent;
    public InputGenerator inputGenerator;
    
    public float shootCooldown = 0.2f;
    public float shootDownImpulse = 1;
    private Shooter weapon;
    private GameObject arm;
    private bool canShoot = true;
    private CharacterControl characterControl;
    private CheckGround checkGround;
    private CheckWall checkWall;

    private void Awake() {
        characterControl = GetComponent<CharacterControl>();
        checkGround = GetComponent<CheckGround>();
        checkWall = GetComponent<CheckWall>();

        if(characterControl == null)
            Debug.Log("No characterControl found by shootController");
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

        if(checkGround != null)
            checkGround.landedEvent += RestoreShoot;

        if(checkWall != null)
            checkWall.walledEvent += RestoreShoot;
    }    
    private void Shoot()
    {
        bool automaticRestore = true;
        if(canShoot && !characterControl.sliding && !checkWall.walled && Time.timeScale > 0)
        {

            weapon.Shoot(inputGenerator.timeOffset);
            canShoot = false;

            if(characterControl != null)
            {

                if(arm.transform.eulerAngles.z == 270)
                {
                    characterControl.Bounce(shootDownImpulse);
                    BlockShoot();
                    automaticRestore = false;
                    if(ShootBounceEvent!=null)
                        ShootBounceEvent.Invoke();
                }else
                {

                    if(ShootEvent!=null)
                        ShootEvent.Invoke();
                }

            }

            if(automaticRestore)
                Invoke("RestoreShoot", shootCooldown);

        }

    }
    private void BufferMovement(float dir)
    {
        if(dir > 0)
        {
            arm.transform.rotation = Quaternion.Euler(0,arm.transform.eulerAngles.y,90);
        }
        else if(dir < 0)
        {
            arm.transform.rotation = Quaternion.Euler(0,arm.transform.eulerAngles.y,-90);
        }
        else
        {
            arm.transform.rotation = Quaternion.Euler(0,arm.transform.eulerAngles.y,0);
        }
    }

    private void RestoreShoot()
    {
        canShoot = true;
    }
    private void RestoreShoot(bool aux)
    {
        RestoreShoot();
    }
    public void BlockShoot()
    {
        canShoot = false;
    }


}
