using UnityEngine;
using GLCore;
using System.Collections;
using UnityEngine.Events;

namespace CityStage
{
    [DisallowMultipleComponent]
    public class TopDown2DSpriteChanger : MonoSingletonBase<TopDown2DSpriteChanger>
    {
        // Refs
        private Animator aniCtrl;        
        //public SpriteRenderer rendererCtrl;
        public TopDown2DCharMoving charCtrl;

        // Animator parameter hashed IDs
        private int aniID_velocity;
        private int aniID_minning;

        private float deltaTime;
        public UnityAction actionAfterMinning;

        private CharacterActionState characterActionState;

        public string velocityParamName;
        public string minningParamName;

        private const int MinningAnimStart = 1;
        private const int MinningAnimStop = 0;

        private Vector3 localEulerAngles;

        public enum CharacterActionState
        {
            idling, minning
        }

        //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

        void Start()
        {
            characterActionState = CharacterActionState.idling;
            aniCtrl = GetComponent<Animator>();

            // Get parameter IDs
            aniID_velocity = Animator.StringToHash(velocityParamName);
            aniID_minning = Animator.StringToHash(minningParamName);
        }

        // Update is called once per frame
        void Update()
        {
            if (characterActionState == CharacterActionState.idling)
            {
                // Parameter setter
                aniCtrl.SetFloat(aniID_velocity, charCtrl.realVelo);

                // When look right side flip X : Sprite Mesh Offset fliper
                localEulerAngles = transform.localEulerAngles;

                if (charCtrl.lookDirRorL == entityLookDir.right)
                {
                    localEulerAngles.y = 180f;
                }
                else if (charCtrl.lookDirRorL == entityLookDir.left)
                {
                    localEulerAngles.y = 0f;
                }

                transform.localRotation = Quaternion.Euler(localEulerAngles);
            }
            else if (characterActionState == CharacterActionState.minning)
            {
                if(charCtrl.velo > 0f)
                {
                    characterActionState = CharacterActionState.idling;
                    aniCtrl.SetInteger(aniID_minning, MinningAnimStop);
                }
                else if (deltaTime > 0f)
                {
                    deltaTime -= Time.deltaTime;
                }
                else
                {
                    characterActionState = CharacterActionState.idling;
                    aniCtrl.SetInteger(aniID_minning, MinningAnimStop);

                    if (actionAfterMinning != null)
                    {
                        actionAfterMinning();
                        actionAfterMinning = null;
                    }
                }
            }
        }

        public void StartMinningAction(float time, UnityAction action, Vector2 orePos)
        {
            characterActionState = CharacterActionState.minning;
            aniCtrl.SetInteger(aniID_minning, MinningAnimStart);

            if((orePos - (Vector2)transform.position).normalized.y > charCtrl.GetLookDirVectorValueY())
            {
                charCtrl.lookDirUorD = entityLookDir.up;
                // 스프라이트 교체 함수
            }
            else
            {
                charCtrl.lookDirUorD = entityLookDir.down;
                // 스프라이트 교체 함수
            }

            if (transform.position.x < orePos.x)
            {
                charCtrl.lookDirRorL = entityLookDir.right;
                localEulerAngles = transform.localEulerAngles;
                localEulerAngles.y = 180f;
                transform.localRotation = Quaternion.Euler(localEulerAngles);
            }
            else
            {
                charCtrl.lookDirRorL = entityLookDir.left;
                localEulerAngles = transform.localEulerAngles;
                localEulerAngles.y = 0f;
                transform.localRotation = Quaternion.Euler(localEulerAngles);
            }

            deltaTime = time;
            actionAfterMinning = action;        
        }
    }
}