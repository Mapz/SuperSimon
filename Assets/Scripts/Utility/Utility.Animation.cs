using System.Linq;
using UnityEngine;
public partial class Utility
{

    //获取Clip名称为name的动画Clip
    public static AnimationClip GetAnimationClip(Animator animator, string name)
    {
        return animator.runtimeAnimatorController.animationClips.First(x => x.name == name);
    }

    public static float GetAnimationClipLength(Animator animator, string name)
    {
        float time = GetAnimationClip(animator, name).length;
        return time;
    }
}