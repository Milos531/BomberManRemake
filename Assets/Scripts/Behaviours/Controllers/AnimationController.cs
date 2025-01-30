using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BEST.BomberMan.Behaviours.Controllers
{
    public class AnimationController : MonoBehaviour
    {
        public static string ANIM_IDLE = "Idle";
        public static string ANIM_WALKING = "Walking";
        public static string ANIM_PLACEBOMB = "PlaceBomb";
        public static string ANIM_DEATH = "Death";

        private Animator characterAnimator;
        private Rigidbody playerRigidbody;
        private EntityController entityController;
        private string currentAnimatonPlaying;
        private bool playingOneOffAnimation;

        private void Start() 
        {
            characterAnimator = GetComponentInChildren<Animator>();
            playerRigidbody = this.GetComponent<Rigidbody>();
            entityController = this.GetComponent<EntityController>();
        }

        private void FixedUpdate() 
        {
            float charVelocity = playerRigidbody.velocity.magnitude;
            if(!entityController.IsDead())
                ChangeAnimationState((charVelocity > 0.2f)? ANIM_WALKING : ANIM_IDLE);
        }

        public void ChangeAnimationState(string newAnimation)
        {
            if (currentAnimatonPlaying == newAnimation || playingOneOffAnimation) return;

            characterAnimator.Play(newAnimation);
            currentAnimatonPlaying = newAnimation;
        }

        public void PlayOneoffAnimation(string newAnimation)
        {
            playingOneOffAnimation = true;
            characterAnimator.Play(newAnimation);
            currentAnimatonPlaying = newAnimation;
        }
    }
}
