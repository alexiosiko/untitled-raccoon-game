using UnityEngine;

public class FirstHouseManager : MonoBehaviour
{
	[SerializeField] int eatenCount = 0;
    private void OnEnable()
    {
        Consumable.OnAnyConsumed += HandleConsumableEaten;
    }

    private void OnDisable()
    {
        Consumable.OnAnyConsumed -= HandleConsumableEaten;
    }

	private void HandleConsumableEaten(Consumable consumable)
    {
        eatenCount++;
        Debug.Log($"FirstHouseManager: {eatenCount} eaten");
        if (eatenCount >= 5)
        {
            // Your logic here
        }
    }

}
