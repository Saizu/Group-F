using UnityEngine;

public static class ItemEffectManager
{
    // Static fields to track item effect usage
    public static bool IsItem1EffectActive { get; private set; } = false;
    public static bool IsItem2EffectActive { get; private set; } = false;

    // Method to activate an item effect
    public static void ActivateItemEffect(int itemNumber)
    {
        switch (itemNumber)
        {
            case 1:
                IsItem1EffectActive = true;
                break;
            case 2:
                IsItem2EffectActive = true;
                break;
            default:
                Debug.LogWarning($"Invalid item number: {itemNumber}");
                break;
        }
    }

    // Method to deactivate an item effect
    public static void DeactivateItemEffect(int itemNumber)
    {
        switch (itemNumber)
        {
            case 1:
                IsItem1EffectActive = false;
                break;
            case 2:
                IsItem2EffectActive = false;
                break;
            default:
                Debug.LogWarning($"Invalid item number: {itemNumber}");
                break;
        }
    }

    // Method to check if an item effect is active
    public static bool IsItemEffectActive(int itemNumber)
    {
        switch (itemNumber)
        {
            case 1:
                return IsItem1EffectActive;
            case 2:
                return IsItem2EffectActive;
            default:
                Debug.LogWarning($"Invalid item number: {itemNumber}");
                return false;
        }
    }

    // Reset method for scene transitions or game reset
    public static void ResetItemEffects()
    {
        IsItem1EffectActive = false;
        IsItem2EffectActive = false;
        Debug.LogWarning("Reset ItemEffects bools.");
    }
}