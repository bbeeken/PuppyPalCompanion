using UnityEngine;

public sealed class CoinDoubler : MonoBehaviour
{
    [SerializeField] private PetManager pet;

    public void DoubleCoins()
    {
        pet.AddCoins(pet.Coins);
    }
}