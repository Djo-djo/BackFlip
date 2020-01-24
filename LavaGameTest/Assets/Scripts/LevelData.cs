using UnityEngine;

[CreateAssetMenu(fileName = "New LevelData", menuName = "Level data", order = 51)]
public class LevelData : ScriptableObject
{
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float jumpPlaceSize;

    public float JumpHeight => jumpHeight;
    public float JumpPlaceSize => jumpPlaceSize;
}