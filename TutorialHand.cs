using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NamPhuThuy.Common;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NamPhuThuy.Tutorial
{
    public class TutorialHand : MonoBehaviour
    {
        [Header("Flags")] 
        [SerializeField] private bool isFollowing = false;
        
        [Header("Components")]
        [SerializeField] private SkeletonGraphic handSkeGraphic;
        [SerializeField] private Image handImage;
        [SerializeField] private Vector3 pivotOffset;
        [SerializeField] private Transform currentTarget;
        
        
        [Header("Animation Names")]
        private string _animAction = "action";
        private string _animAction2 = "action2";
        private string _animBegin1 = "begin1";
        private string _animBegin2 = "begin2";
        private string _animEnd1 = "end1";
        private string _animEnd2 = "end2";
        
        /// <summary>
        /// The hand move except finger
        /// </summary>
        public string AnimAction => _animAction;
        
        /// <summary>
        /// Finger move
        /// </summary>
        public string AnimAction2 => _animAction2;
        public string AnimBegin1 => _animBegin1;
        public string AnimBegin2 => _animBegin2;
        public string AnimEnd1 => _animEnd1;
        public string AnimEnd2 => _animEnd2;
        
        
        #region Private Serializable Fields

        #endregion

        #region Private Fields

        #endregion

        #region MonoBehaviour Callbacks

        #endregion

        #region Public Methods
        
        private RectTransform rectTransform;

       
        public void EnableHand()
        {
            DebugLogger.Log();
            if (handImage != null)
            {
               handImage.gameObject.SetActive(true);
            }

            if (handSkeGraphic != null)
            {
                handSkeGraphic.gameObject.SetActive(true);
            }
        }

        public void DisableHand()
        {
            DebugLogger.Log();
            if (handImage != null)
            {
                handImage.gameObject.SetActive(false);
            }

            if (handSkeGraphic != null)
            {
                handSkeGraphic.gameObject.SetActive(false);
            }
        }

        public void FollowTransform(Transform tartget)
        {
            isFollowing = true;
            currentTarget = tartget;
        }

        public void UnfollowTransform(Transform tartget)
        {
            isFollowing = false;
            currentTarget = null;
        }

        #endregion

        #region Hand Control
        
        public void SetWorldPosition(Vector3 worldPos)
        {
            DebugLogger.Log();
            transform.position = worldPos + pivotOffset;
        }

        public void SetScreenPosition(Vector2 screenPos)
        {
            DebugLogger.Log();
            
            // treat pivotOffset as screen\-space offset (x,y). z is ignored.
            Vector3 screenWithOffset = new Vector3(
                screenPos.x + pivotOffset.x,
                screenPos.y + pivotOffset.y,
                pivotOffset.z
            );
            
            if (rectTransform != null)
            {
                rectTransform.position = screenWithOffset;
            }
            else
            {
                transform.position = screenWithOffset;
            }
        }


        public void MoveHandToWorldObject(Transform targetTransform)
        {
            DebugLogger.Log();
            if (targetTransform == null)
            {
                DebugLogger.Log(message: $"Return");
                return;
            }
            SetWorldPosition(targetTransform.position);
        }

        public void MoveToScreenPointFromWorldFast(Vector3 worldPosition)
        {
            DebugLogger.Log();
            if (Camera.main == null)
            {
                DebugLogger.Log(message: $"Return");
                return;
            }
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
            SetScreenPosition(screenPos);
        }
        
        public void MoveToScreenPointFromWorldTween(Vector3 worldPosition, float duration = 0.5f)
        {
            DebugLogger.Log();
            if (Camera.main == null)
            {
                DebugLogger.Log(message: "Return");
                return;
            }

            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
            
            // apply offset before tween
            Vector3 screenWithOffset = new Vector3(
                screenPos.x + pivotOffset.x,
                screenPos.y + pivotOffset.y,
                pivotOffset.z
            );

            // Kill any existing tween on this transform/rectTransform if needed
            if (rectTransform != null)
            {
                rectTransform.DOKill();
                rectTransform.DOMove(screenWithOffset, duration);
            }
            else
            {
                transform.DOKill();
                transform.DOMove(screenWithOffset, duration);
            }
        }

        #endregion
        
        #region Private Methods
        
        public void PlayAnimation(string animName, bool loop = true)
        {
            DebugLogger.Log(message: $"PlayAnimation: {animName}, loop: {loop}");
            if (handSkeGraphic != null)
            {
                DebugLogger.Log(message: $"Play Animation: {animName}, loop: {loop}");
                handSkeGraphic.AnimationState.SetAnimation(0, animName, loop);
            }
        }
        
        #endregion

        #region Editor Methods

        public void ResetValues()
        {
            
        }

        #endregion
    }

    /*#if UNITY_EDITOR
    [CustomEditor(typeof(TutorialHand))]
    [CanEditMultipleObjects]
    public class TutorialHandEditor : Editor
    {
        private TutorialHand script;
        private Texture2D frogIcon;
        
        private void OnEnable()
        {
            frogIcon = Resources.Load<Texture2D>("frog"); // no extension needed
        }
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            script = (TutorialHand)target;

            ButtonResetValues();
        }

        private void ButtonResetValues()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(new GUIContent("Reset Values", frogIcon), GUILayout.Width(InspectorConst.BUTTON_WIDTH_MEDIUM)))
            {
                script.ResetValues();
                EditorUtility.SetDirty(script); // Mark the object as dirty
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
    #endif*/
}