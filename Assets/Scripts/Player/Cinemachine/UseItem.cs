using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Audio;
using Gravity;
using Interfaces;
using Planets;

namespace Player
{
    public class UseItem : MonoBehaviour
    {
        private Animator _animator;
        private GravityControl _gravityControl;
        private MoveCineMachine playerMovement;
        private Rigidbody _rigidbody;

        [Header("Items")]
        public PlayerItems playerItems;
        public Transform firePoint;
        public float reloadTime = 5f;

        //jetPack
        public bool hasJetpack;
        public GameObject jetPack;
        private bool _canJetPack = true;

        //stick
        public bool hasStick;
        public GameObject stickObj;
        public GameObject stickKnockBack;
        private bool _canSwingStick = true;

        //freezeRay
        public bool hasFreezeRay;
        public GameObject freezeRayGun;
        public GameObject freezeRayProjectile;
        public float freezeTime = 10f;
        private bool _canShootFreezeRay = true;

        //rocketLauncher
        public bool hasRocketLauncher;
        public GameObject rocketLauncher;
        public GameObject rocket;
        public float launchForce = 50f;
        private bool _canShootRocket = true;

        //speedboost
        public bool hasSpeedIncrease;
        public GameObject energyCan;
        public float speedMultiplier = 2f;
        private bool _canSprint = true;
        [HideInInspector]
        public bool isSprinting;


        [Header("Particles")]
        public ParticleSystem landParticles;
        private bool _canPlayLandParticles = true;
        public ParticleSystem jumpParticles;
        public ParticleSystem walkParticles;
        public ParticleSystem jetParticles;
        public ParticleSystem speedTrail;
        public ParticleSystem frozenParticles;

        // Knockback-related 
        [HideInInspector]
        public bool inKnockBack;
        private const float KnockBackTime = .75f;
        private float _knockBackCounter;

        private static readonly int IsGrounded = Animator.StringToHash("isGrounded");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Falling = Animator.StringToHash("Falling");
        private static readonly int Attack = Animator.StringToHash("SwingAttack");
        private static readonly int Shoot = Animator.StringToHash("ShootPistol");
        private static readonly int Drink = Animator.StringToHash("Drink");
        // Start is called before the first frame update
        void Awake()
        {
            jetParticles.Stop();
            speedTrail.Stop();
            frozenParticles.Stop();
            _canPlayLandParticles = true;

            // Initialize components
            _animator = GetComponentInChildren<Animator>();
            playerMovement = GetComponent<MoveCineMachine>();
            _rigidbody = GetComponentInChildren<Rigidbody>();

            _canPlayLandParticles = true;

        }

        // Update is called once per frame
        void Update()
        {
            hasStick = false;
            hasJetpack = false;
            hasFreezeRay = false;
            hasRocketLauncher = false;
            hasSpeedIncrease = false;

            switch (playerItems.selectedItem)
            {
                case PlanetType.None:
                    break;
                case PlanetType.Stick:
                    hasStick = true;
                    break;
                case PlanetType.Jetpack:
                    hasJetpack = true;
                    break;
                case PlanetType.FreezeGun:
                    hasFreezeRay = true;
                    break;
                case PlanetType.RocketLauncher:
                    hasRocketLauncher = true;
                    break;
                case PlanetType.SpeedIncrease:
                    hasSpeedIncrease = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //activates physical items based on bools
            stickObj.SetActive(hasStick);
            jetPack.SetActive(hasJetpack);
            freezeRayGun.SetActive(hasFreezeRay);
            rocketLauncher.SetActive(hasRocketLauncher);
            energyCan.SetActive(hasSpeedIncrease);

            if (PauseManager.Instance == null) return;
            if (PauseManager.Instance.isPaused) return;
        }
        public void UseItemAction(InputAction.CallbackContext context)
        {
            if (hasJetpack && context.started && _canJetPack && !inKnockBack)
                StartCoroutine(JetPack());
            else if (hasStick && context.started && _canSwingStick)  // If the key was not pressed this frame, ignore it.
                StartCoroutine(SwingAnimation());
            else if (hasSpeedIncrease && _canSprint && !inKnockBack)
                StartCoroutine(Sprint());
            else if (hasRocketLauncher && _canShootRocket && context.started)
                StartCoroutine(ShootRocketLauncher());
            else if (hasFreezeRay && _canShootFreezeRay && context.started)
                StartCoroutine(ShootFreezeRay());

        }
        private IEnumerator JetPack()
        {
            // Apply force while jetpack input is activated
            _canJetPack = false;
            AudioManager.Instance.fx.JetPack();
            jetParticles.Play(); // Needs edits based on hold

            _animator.SetBool(IsGrounded, playerMovement.isGrounded);
            _rigidbody.AddForce(transform.up * (playerMovement.JumpForce));
            if (playerMovement._movementInput.y > 0)
                _rigidbody.AddForce(transform.forward * (playerMovement.JumpForce * 2f));
            else if (playerMovement._movementInput.y < 0)
                _rigidbody.AddForce(transform.forward * (playerMovement.JumpForce * -2f)); // if moving backward apply force backward


            _animator.SetTrigger(Falling); // Transitions walking animation to falling without having to go through Jump
            Debug.Log("jetpack");
            yield return new WaitForSeconds(.5f);
            _canJetPack = true;
            _gravityControl.NearestPlanet();
        }

        private IEnumerator SwingAnimation()
        {
            // Player cannot swing stick again until animation plays through
            _canSwingStick = false;
            _canPlayLandParticles = false;
            _animator.SetTrigger(Attack);
            yield return new WaitForSeconds(.5f);
            AudioManager.Instance.fx.StickSwoosh();

            // Activates stick and deactivates after the animation plays out
            stickKnockBack.SetActive(true);
            yield return new WaitForSeconds(1f);
            stickKnockBack.SetActive(false);
            _canSwingStick = true;
            _canPlayLandParticles = true;

        }

        private IEnumerator Sprint()
        {
            // Doubles player speed for short period, then has cooldown period before can be used again
            _animator.SetTrigger(Drink);

            _canSprint = false;
            AudioManager.Instance.fx.EnergyDrink();
            speedTrail.Play();
            isSprinting = true;
            playerMovement._walkSpeed *= speedMultiplier;
            yield return new WaitForSeconds(20f);
            playerMovement._walkSpeed /= speedMultiplier;
            speedTrail.Stop();

            yield return new WaitForSeconds(10f);
            _canSprint = true;
            isSprinting = false;

        }
        private void ApplyKnockBack(Vector3 direction, float force)
        {
            // Takes in Vector3 direction value, applies force
            _knockBackCounter = KnockBackTime;
            playerMovement._moveDirection = direction * force;
            playerMovement._moveDirection.y = 2f;
        }
        private void OnTriggerEnter(Collider other)
        {
            // Checks Trigger, and starts knockback sequence.
            if (other.gameObject.CompareTag("KnockBack"))
            {
                Vector3 hitDirection = other.transform.position - transform.position;
                hitDirection = hitDirection.normalized;
                ApplyKnockBack(hitDirection, 10f);
                _animator.SetTrigger(Falling);
            }

            // Landing particles
            if (other.gameObject.CompareTag("InnerGravity") && _canPlayLandParticles)
                landParticles.Play();

        }
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("FreezeRay"))
                StartCoroutine(FreezePlayer());
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Rocket")) return;

            //applies more knockback than stick
            Vector3 hitDirection = collision.transform.position - transform.position;
            hitDirection = hitDirection.normalized;
            ApplyKnockBack(hitDirection, 20f);
            _animator.SetTrigger(Falling);
        }

        public void AddForce(Vector3 force, ForceMode forceMode)
        {
            _rigidbody.AddForce(force, forceMode);
        }
        private IEnumerator ShootRocketLauncher()
        {
            AudioManager.Instance.fx.RocketLaunch();

            _canShootRocket = false;
            _animator.SetTrigger(Shoot);
            // _audio.RocketLaunch();

            //instatiates projectile and adds velocity
            var projectileObj = Instantiate(rocket, firePoint.position, Quaternion.identity);
            projectileObj.GetComponent<Rigidbody>().velocity =
                transform.TransformDirection(Vector3.forward * (launchForce));

            //wait before can shoot again
            yield return new WaitForSeconds(reloadTime);
            _canShootRocket = true;
        }
        private IEnumerator ShootFreezeRay()
        {
            AudioManager.Instance.fx.FreezeRayLaunch();

            //identical to rocket launcher but shoots freezeRay projectile
            _canShootFreezeRay = false;
            _animator.SetTrigger(Shoot);
            //_audio.FreezeRayLaunch();


            //instatiates projectile and adds velocity
            var projectileObj = Instantiate(freezeRayProjectile, firePoint.position, Quaternion.identity);
            projectileObj.GetComponent<Rigidbody>().velocity =
                transform.TransformDirection(Vector3.forward * (launchForce));

            //wait before can shoot again
            yield return new WaitForSeconds(reloadTime);
            _canShootFreezeRay = true;
        }
        private IEnumerator FreezePlayer()
        {
            //slows down player for alloted time
            if (!isSprinting)
                playerMovement._walkSpeed = 3f;
            else
                playerMovement._walkSpeed = 3f * speedMultiplier;
            frozenParticles.Play();

            yield return new WaitForSeconds(freezeTime);
            if (!isSprinting)
                playerMovement._walkSpeed = 15f;
            else
                playerMovement._walkSpeed = 15f * speedMultiplier;
            frozenParticles.Stop();

        }
    }
}
