using UnityEngine.EventSystems;
public interface UpgradeMessageTarget : IEventSystemHandler {
    // functions that can be called via the messaging system
    void OnUpgradeMessage (int changeLv);
    void OnHpChangeMessage (int changeHP);
}