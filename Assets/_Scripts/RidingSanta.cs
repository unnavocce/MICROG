using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.LEGO.Minifig
{
    public class RidingSanta : MinifigController
    {
        // Constants.
        const float slowDownDuration = 1.0f;
        const float alwaysRunSpeed = 12.0f;

        public new enum SpecialAnimation
        {
            Bow,
            Dance,
            Fart,
            Gallop,
            Idle,
            Jump,
            Look,
            Maybe,
            No,
            Trot,
            Yes
        }

        float time;
        Animator[] animators;

        protected override void OnValidate()
        {
            maxForwardSpeed = Mathf.Clamp(maxForwardSpeed, 5, 30);

            if (!leftArmTip || !rightArmTip || !leftLegTip || !rightLegTip || !head)
            {
                var minifig = GetComponentInChildren<Minifig>();
                if (minifig)
                {
                    var rigTransform = minifig.transform.GetChild(1);
                    if (rigTransform)
                    {
                        FindJointReferences(rigTransform);
                    }
                    else
                    {
                        Debug.LogError("Failed to find Minifigure rig.");
                    }
                }
                else
                {
                    Debug.LogError("Failed to find a Minifigure.");
                }
            }
        }

        protected override void Awake()
        {
            minifig = GetComponentInChildren<Minifig>();
            controller = GetComponent<CharacterController>();
            audioSource = GetComponent<AudioSource>();
            animators = GetComponentsInChildren<Animator>();
            animator = GetComponent<Animator>();

            // Initialise animation.
            foreach (var animator in animators)
            {
                animator.SetBool(groundedHash, true);
            }

            // Make sure the Character Controller is grounded if starting on the ground.
            if (controller.enabled)
            {
                controller.Move(Vector3.down * 0.01f); 
            }
        }

        protected override void Update()
        {
            // Handle input.
            if (!exploded)
            {
                if (inputEnabled)
                {
                    speed = alwaysRunSpeed;
                    rotateSpeed = 0.0f;
                    moveDelta = new Vector3(0, moveDelta.y, 0);

                    // Check if player is grounded.
                    if (!airborne)
                    {
                        jumpsInAir = maxJumpsInAir;
                    }

                    // Check if player is jumping.
                    if (Input.GetButtonDown("Jump"))
                    {
                        if (!airborne || jumpsInAir > 0)
                        {
                            if (airborne)
                            {
                                jumpsInAir--;

                                if (doubleJumpAudioClip)
                                {
                                    audioSource.PlayOneShot(doubleJumpAudioClip);
                                }
                            }
                            else
                            {
                                if (jumpAudioClip)
                                {
                                    audioSource.PlayOneShot(jumpAudioClip);
                                }
                            }

                            moveDelta.y = jumpSpeed;

                            foreach (var animator in animators)
                            {
                                animator.SetTrigger(jumpHash);
                            }

                            airborne = true;
                            airborneTime = coyoteDelay;
                        }
                    }

                    // Cancel special.
                    cancelSpecial = Input.GetButtonDown("Jump");
                }
                else
                {
                    // Handle automatic animation (if needed).
                }
            }
            else
            {
                // Slow down speed and movement to 0 over time.
                time += Time.deltaTime;
                speed = Mathf.Lerp(speed, 0f, time / slowDownDuration);
                rotateSpeed = Mathf.Lerp(rotateSpeed, 0f, time / slowDownDuration);
                moveDelta.x = Mathf.Lerp(moveDelta.x, 0f, time / slowDownDuration);
                moveDelta.z = Mathf.Lerp(moveDelta.z, 0f, time / slowDownDuration);
            }

            HandleMotion();
        }
        public void Jump()
        {
            if (!airborne || jumpsInAir > 0)
            {
                if (airborne)
                {
                    jumpsInAir--;

                    if (doubleJumpAudioClip)
                    {
                        audioSource.PlayOneShot(doubleJumpAudioClip);
                    }
                }
                else
                {
                    if (jumpAudioClip)
                    {
                        audioSource.PlayOneShot(jumpAudioClip);
                    }
                }

                moveDelta.y = jumpSpeed;

                foreach (var animator in animators)
                {
                    animator.SetTrigger(jumpHash);
                }

                airborne = true;
                airborneTime = coyoteDelay;
            }
        }

        protected override void UpdateMotionAnimations()
        {
            // Update animation - delay airborne animation slightly to avoid flailing arms when falling a short distance.
            foreach (var animator in animators)
            {
                animator.SetBool(cancelSpecialHash, cancelSpecial);
                animator.SetFloat(speedHash, speed);
                animator.SetFloat(rotateSpeedHash, rotateSpeed);
                animator.SetBool(groundedHash, !airborne);
            }
        }

        public void PlaySpecialAnimation(SpecialAnimation animation, AudioClip specialAudioClip = null, Action<bool> onSpecialComplete = null)
        {
            foreach (var animator in animators)
            {
                animator.SetBool(playSpecialHash, true);
                animator.SetInteger(specialIdHash, (int)animation);
            }

            if (specialAudioClip)
            {
                audioSource.PlayOneShot(specialAudioClip);
            }

            this.onSpecialComplete = onSpecialComplete;
        }

        public override void Explode()
        {
            if (!exploded)
            {
                exploded = true;

                // Move Minifigure to root hierarchy.
                minifig.transform.parent = null;
                
                // Find and disable Minifigure animator.
                foreach (var animator in animators)
                {
                    if (animator.gameObject == minifig.gameObject)
                    {
                        animator.enabled = false;
                    }
                }

                HandleExplode();
            }
        }
    }
}
