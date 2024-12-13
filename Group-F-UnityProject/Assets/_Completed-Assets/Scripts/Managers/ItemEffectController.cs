using UnityEngine;

public class ItemEffectController : MonoBehaviour
{
    public int itemNumber; // Set this in the inspector (1 or 2)

    private void OnDestroy()
    {
        // When the effect object is destroyed, deactivate the effect state
        ItemEffectManager.DeactivateItemEffect(itemNumber);
        Debug.Log($"Item {itemNumber} effect deactivated");
    }
}
