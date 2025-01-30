using System;
using System.Collections;
using BEST.BomberMan.Core;
using BEST.BomberMan.Management;
using BEST.BomberMan.ScriptableObjects;
using UnityEngine;

namespace BEST.BomberMan.Behaviours.Controllers
{
    public class PlayerController : EntityController, IHittable
    {
        [SerializeField]
        private float speed;
        [SerializeField]
        private float PlacingBombTime;

        [field: SerializeField] public GameObject BombPrefab;
        [field: SerializeField] public BombData DefaultBombData;

        private BombData currentBombData;
        private Rigidbody playerRigidbody;
        private AnimationController animationController;
        private LevelManager levelManager;
        public InputManager inputManager;
        private GameManager gameManager;

        private bool iframesActive { get; set; }
        public bool isPlacingBomb { get; set; }

        public Vector3 Location { get; set; }
        [field:SerializeField] public HittableType HittableType { get; set; }

        public event Action<BombData> OnBombChanged;
        public event Action OnDeath;

        private void Start()
        {
            inputManager.MovementInputReceived += InputManager_OnMovementInputReceived;
            inputManager.Fire += InputManager_OnFirePressed;
            playerRigidbody = this.GetComponent<Rigidbody>();

            isDead = false;
            iframesActive = false;
            currentBombData = DefaultBombData;
            animationController = GetComponentInChildren<AnimationController>();
            levelManager = FindObjectOfType<LevelManager>();
            gameManager = FindObjectOfType<GameManager>();
        }


        private void InputManager_OnFirePressed()
        {
            if(isDead) return;
            
            PlaceBomb();
        }

        private void InputManager_OnMovementInputReceived(float horizontal, float vertical)
        {
            if(isDead) return;
            
            if (horizontal != 0)
            {
                playerRigidbody.AddForce(new Vector3(horizontal, 0, 0) * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
                playerRigidbody.MoveRotation(Quaternion.Euler(Vector3.up * 90 * horizontal));
            }
            else if (vertical != 0)
            {
                playerRigidbody.AddForce(new Vector3(0, 0, vertical) * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);

                playerRigidbody.MoveRotation(vertical > 0 
                    ? Quaternion.Euler(Vector3.zero)
                    : Quaternion.Euler(Vector3.up * 180 * vertical));
            }

            Location = this.transform.position;
        }

        public void AcquireBomb(BombData newBombData)
        {
            currentBombData = newBombData;
            OnBombChanged?.Invoke(currentBombData);
        }

         public void PlaceBomb(){
            isPlacingBomb = true;
            if(animationController != null){
                animationController.ChangeAnimationState(AnimationController.ANIM_PLACEBOMB);
            }
            StartCoroutine(AnimatePlacingBomb());

            Node<ILevelBlock> node = levelManager.WorldPositionToNode(transform.position);
            var bomb = Instantiate(BombPrefab, levelManager.NodeToWorldPosition(node), Quaternion.identity);
            bomb.GetComponent<BombController>().InitializeBombData(currentBombData, node, this);
        }

        IEnumerator AnimatePlacingBomb(){
            yield return new WaitForSeconds(PlacingBombTime);
            isPlacingBomb = false;
        }

        public void RecieveHit()
        {
            Die();
        }

        public void Die()
        {
            isDead = true;
            OnDeath?.Invoke();
            if(animationController != null){
                animationController.ChangeAnimationState(AnimationController.ANIM_DEATH);
            }
            gameManager.GameOver(false);
               
        }
    }
}

