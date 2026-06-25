using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunShotAnimation : MonoBehaviour
{
    [Header("Animatie")]
    public Animator gunAnimator;      

    public StudioEventEmitter gunShotEmmiter;

    [Header("Particle System")]
    public ParticleSystem muzzleFlash; 
    
    private bool isShooting = false;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !isShooting)
        {
            Shoot();
        }
    }

    public void ParticlesShoot()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
    }

    public void Shoot()
    {
        isShooting = true;

        if (gunAnimator != null)
        {
            gunAnimator.SetTrigger("shootTrigger");
            gunShotEmmiter.Play();
        }

        float shootAnimLength = gunAnimator.GetCurrentAnimatorStateInfo(0).length/2;
        Invoke(nameof(ResetShoot), shootAnimLength);
    }

    private void ResetShoot()
    {
        isShooting = false;
    }
}
