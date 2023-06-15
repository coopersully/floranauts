using System;
using System.Collections;
using Audio;
using Gravity;
using Interfaces;
using Planets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Cinemachine
{
    public class UseItem : MonoBehaviour
    {
        private Animator _animator;
        private GravityControl _gravityControl;
        private CameraRotation _cameraRotation;
        private MoveCineMachine _playerMovement;
        private Rigidbody _rigidbody;
        //aim mechanics
        public Camera camera;
        public GameObject aimCursor;
        Ray aimRay;
        Ray cameraRay;
        private Vector3 destination;
        private bool targetFound;

        [Header("Items")]
        public PlayerItems playerItems;
        public Transform firePoint;
        public float reloadTime = 5f;
        public enum ProjectileType
        {
            rocket,
            freeze
        }
        //public ProjectileType projectileType;

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
            _playerMovement = GetComponent<MoveCineMachine>();
            _cameraRotation = GetComponent<CameraRotation>();
            _rigidbody = GetComponentInChildren<Rigidbody>();

            _canPlayLandParticles = true;
            

        }

        // Update is called once per frame
        void Update()
        {
            hasStick = false;
            hasJetpack = false;
            //hasFreezeRay = false;
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
                ShootGun(ProjectileType.rocket);
            //StartCoroutine(ShootRocketLauncher());
            else if (hasFreezeRay && _canShootFreezeRay && context.started)
                ShootGun(ProjectileType.freeze);
            //StartCoroutine(ShootFreezeRay());
            

        }
        private IEnumerator JetPack()
        {
            // Apply force while jetpack input is activated
            _canJetPack = false;
            AudioManager.Instance.fx.JetPack();
            jetParticles.Play(); // Needs edits based on hold

            _animator.SetBool(IsGrounded, _playerMovement.isGrounded);
            _rigidbody.AddForce(transform.up * (_playerMovement.JumpForce));
            if (_playerMovement._movementInput.y > 0)
                _rigidbody.AddForce(transform.forward * (_playerMovement.JumpForce * 2f));
            else if (_playerMovement._movementInput.y < 0)
                _rigidbody.AddForce(transform.forward * (_playerMovement.JumpForce * -2f)); // if moving backward apply force backward


            _animator.SetTrigger(Falling); // Transitions walking animation to falling without having to go through Jump
            Debug.Log("jetpack");
            yield return new WaitForSeconds(.5f);
            _canJetPack = true;
            _gravityControl.AttractToNearestPlanet();
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
            _playerMovement._walkSpeed *= speedMultiplier;
            yield return new WaitForSeconds(20f);
            _playerMovement._walkSpeed /= speedMultiplier;
            speedTrail.Stop();

            yield return new WaitForSeconds(10f);
            _canSprint = true;
            isSprinting = false;

        }
        private void ApplyKnockBack(Vector3 direction, float force)
        {
            // Takes in Vector3 direction value, applies force
            _playerMovement._moveDirection = direction * force;
            _playerMovement._moveDirection.y = 2f;
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
            //AudioManager.Instance.fx.RocketLaunch();

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
                _playerMovement._walkSpeed = 3f;
            else
                _playerMovement._walkSpeed = 3f * speedMultiplier;
            frozenParticles.Play();

            yield return new WaitForSeconds(freezeTime);
            if (!isSprinting)
                _playerMovement._walkSpeed = 15f;
            else
                _playerMovement._walkSpeed = 15f * speedMultiplier;
            frozenParticles.Stop();

        }
        public void ShootGun(ProjectileType projectileType)
        {
            //_cameraRotation.inAim = true; //orients player to face the direction of the camera

            //shoots ray from camera and sets hit point as target
            cameraRay.origin = camera.transform.position;
            cameraRay.direction = (aimCursor.transform.position - camera.transform.position).normalized;
            aimRay.origin = cameraRay.GetPoint(50);//establish aiming ray as point in front of player
            aimRay.direction = (aimRay.origin - aimCursor.transform.position).normalized;

            /**
            Debug.DrawLine(cameraRay.origin, cameraRay.GetPoint(1000) + cameraRay.direction, Color.red,5);
            Debug.DrawLine(aimRay.origin, aimRay.GetPoint(1000) + aimRay.direction, Color.green,5);
            **/

            RaycastHit hit;
            if (Physics.Raycast(aimRay.origin, aimRay.direction, out hit))
            {
                //Debug.Log("hit " + hit.transform.tag);
                destination = hit.point;
                targetFound = true;
            }
            else
            {
                destination = aimRay.GetPoint(1000);
                targetFound = false;
            }

            //pass projectile object into InstantiateProjectile() depending on what type of gun it is
            switch (projectileType)
            {
                case ProjectileType.rocket:
                    InstantiateProjectile(rocket, targetFound);
                    break;
                case ProjectileType.freeze:
                    InstantiateProjectile(freezeRayProjectile, targetFound);
                    break;
            }
            Debug.Log("target found = " + targetFound);
        }
        void InstantiateProjectile(GameObject projectile, bool targetFound)
        {
            //spawn projectile and apply force towards target
            var projectileObj = Instantiate(projectile, firePoint.position, Quaternion.identity) as GameObject;
            projectileObj.GetComponent<RocketProjectile>().targetFound = targetFound;
            projectileObj.GetComponent<Rigidbody>().velocity = (destination - firePoint.position).normalized * launchForce;
        }
        

    }
}
