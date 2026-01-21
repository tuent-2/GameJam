using System;
using System.Collections;
using UnityEngine;

namespace Game.HotUpdateScripts.Utils
{
    public abstract class BaseAnimationMonoBehaviour : MonoBehaviour
    {
        [SerializeField] protected Animator animator;
        private Coroutine _onAnimationEnd;

        protected string CurrentState { get; private set; }

        protected AnimationClip GetCurrentAnimationClip => animator.GetCurrentAnimatorClipInfo(0)[0].clip;

        private void OnValidate()
        {
            if (animator)
                animator = GetComponentInChildren<Animator>();
        }

        protected IEnumerator SetAnimationState(string newState, Action onAnimationEnd = null)
        {
            Debug.Log($"Play anim: {newState}");
            animator.Play(newState);
            yield return Helpers.GetWaitForSeconds(0.1f);
            var clip = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
            if (_onAnimationEnd != null) StopCoroutine(_onAnimationEnd);
            if (!clip.isLooping) _onAnimationEnd = StartCoroutine(IEOnAnimationEnd());

            IEnumerator IEOnAnimationEnd()
            {
                yield return Helpers.GetWaitForSeconds(clip.length);
                onAnimationEnd?.Invoke();
            }
        }
    }
}