using DG.Tweening;
using UnityEngine;
public class ItemFromBrick : Item
{
    protected override void _Awake()
    {
        transform.DOMoveY(10, 0.5f).SetRelative().OnComplete(() =>
        {
            GetComponent<Collider2D>().enabled = true;
            CreateTriggerItem();
        }).SetEase(Ease.Linear);
    }
}