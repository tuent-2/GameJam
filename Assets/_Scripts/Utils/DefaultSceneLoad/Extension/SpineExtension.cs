using Spine;
using Spine.Unity;

public static class SpineExtension
{
    public static void SetAnimation(this SkeletonGraphic skeletonGraphic, string nameOfAnimation,
        bool isLoop = true)
    {
        skeletonGraphic.AnimationState.SetAnimation(0, nameOfAnimation, isLoop);
    }

    public static void AddAnimation(this SkeletonGraphic skeletonGraphic, string nameOfAnimation,
        bool isLoop = true)
    {
        skeletonGraphic.AnimationState.AddAnimation(0, nameOfAnimation, isLoop, 0);
    }

    public static void OnComplete(this SkeletonGraphic skeletonGraphic, AnimationState.TrackEntryDelegate onComplete)
    {
        skeletonGraphic.AnimationState.Complete += onComplete;
    }
}