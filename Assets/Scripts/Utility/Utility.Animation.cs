using System.Linq;
using UnityEngine;
public partial class Utility
{




}

public static class AnimationHelper
{

    //获取Clip名称为name的动画Clip
    public static AnimationClip GetAnimationClip(this Animator animator, string name)
    {
        return animator.runtimeAnimatorController.animationClips.First(x => x.name == name);
    }

    public static float GetAnimationClipLength(this Animator animator, string name)
    {
        float time = GetAnimationClip(animator, name).length;
        return time;
    }

    public static bool IsAnimationState(this Animator animator, string name, int layer = 0)
    {
        var id = Animator.StringToHash(name);
        if (!animator.HasState(layer, id))
        {
            Debug.LogError("The animator does not have state: " + name);
            return false;
        }
        var state = animator.GetCurrentAnimatorStateInfo(layer);
        if (state.fullPathHash == id || state.shortNameHash == id)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void PlayAvoidRePlay(this Animator animator, string name)
    {
        if (!animator.IsAnimationState(name))
        {
            animator.Play(name);
        }
    }
}