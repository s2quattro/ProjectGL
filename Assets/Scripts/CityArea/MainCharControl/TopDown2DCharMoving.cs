using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLCore;
using Kaibrary;


namespace CityStage
{
    [DisallowMultipleComponent]
    public class TopDown2DCharMoving : MonoBehaviour
    {
        // Refs
        public AdaptedJoystick joystick;
		public Rigidbody2D rigidbodyCtrl;

        // lookAt Object
        public Transform mainCharLookTarget;
        public Vector2 lookingVector { get { return mainCharLookTarget.localPosition.normalized; } }
        public float velo;
		public float realVelo;

		[Header("Option")]
        public float moveSpeed;

		// Inner field
        private float accumulateVelo = 0f;
        private const float staminaUseVeloPoint = 100f;
		private Vector2 previousVelo;

        private LocalStorage localStorage;
        private CharActionMetadata charActionMetadata;

        //Look direction
        [HideInInspector] public entityLookDir lookDirUorD;
        [HideInInspector] public entityLookDir lookDirRorL;

        //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

        private void Start()
        {
            localStorage = LinkContainer.Instance.localStorage;
            charActionMetadata = LinkContainer.Instance.charActionMetadata;

			// Init SpeedMeter
			previousVelo = transform.position;
		}

        void FixedUpdate()
        {
            // from joystick
            Vector3 moveVector = (Vector3.right * joystick.Horizontal + Vector3.up * joystick.Vertical);

            if ((moveVector.x <= 0.1f) && (moveVector.x >= 0f))
            {
                moveVector.x = 0f;
            }
            else if ((moveVector.x >= -0.1f) && (moveVector.x <= 0f))
            {
                moveVector.x = 0f;
            }
            if ((moveVector.y <= 0.1f) && (moveVector.y >= 0f))
            {
                moveVector.y = 0f;
            }
            else if ((moveVector.y >= -0.1f) && (moveVector.y <= 0f))
            {
                moveVector.y = 0f;
            }

            if (moveVector != Vector3.zero)
            {
                moveSpeed = charActionMetadata.defaultMoveSpeed * (1f + StatFactorManager.Instance.GetMoveSpeed);

                if (localStorage.playerStat.GetCurStamina <= 0d)
                {
                    moveSpeed *= charActionMetadata.moveSpeedMultWhenStaminaZero;
                }                

                // SpeedMeter
                velo = moveVector.magnitude * moveSpeed;
				realVelo = Vector2.Distance(previousVelo, transform.position) * moveSpeed;
				previousVelo = transform.position;


				if (!StatFactorManager.Instance.GetNoStaminaConsumtionWhenMoving)
                {
                    accumulateVelo += velo;
                }

                // Actual moving part
                transform.Translate(moveVector * moveSpeed * Time.fixedDeltaTime, Space.World);

                changeLookDir();
                checkLookDir();
                //drawSightLine();

                if(accumulateVelo >= staminaUseVeloPoint)
                {
                    accumulateVelo -= staminaUseVeloPoint;
                    StaminaManager.Instance.UseStamina(CharActionType.Moving);
                }
            }
            else
            {
                // SpeedMeter
                velo = 0f;
				realVelo = 0f;
			}
        }

        private void changeLookDir()
        {
            mainCharLookTarget.localPosition = joystick.Direction.normalized * 3f;
        }

        public float GetLookDirVectorValueY()
        {
            return VectorTreatTools.rotate(Vector2.up, 65f).normalized.y;
        }

        private void checkLookDir()
        {
            //if (Vector2.Dot(lookingVector, Vector2.up) > 0f)
            if (lookingVector.y > VectorTreatTools.rotate(Vector2.up, 65f).normalized.y)

            {
                lookDirUorD = entityLookDir.up;
            }
            else
            {
                lookDirUorD = entityLookDir.down;
            }


            if (Vector2.Dot(lookingVector, Vector2.right) > 0f)
            {
                lookDirRorL = entityLookDir.right;
            }
            else
            {
                lookDirRorL = entityLookDir.left;
            }
        }


        //For Debug
        //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

        /*
        private void OnDrawGizmos()
        {	
            Gizmos.DrawLine(transform.position, mainCharLookTarget.position);
        }	
        private void drawLookAt()
        {
            //Debug.DrawLine(transform.position, mainCharLookDir.position, Color.blue);		
        }*/
        private void drawSightLine()
        {
            Debug.DrawLine(transform.position, (Vector2)transform.position + VectorTreatTools.rotate(lookingVector, 65f) * 1.22f, Color.blue);
            Debug.DrawLine(transform.position, (Vector2)transform.position + VectorTreatTools.rotate(lookingVector, -65f) * 1.22f, Color.red);
        }

    }
}