using UnityEngine;

[CreateAssetMenu(fileName = "RarityConfig", menuName = "Game/RarityConfig")]
public class RarityConfig : ScriptableObject
{
    public Rarity rarity;
    public Color color;
    public GameObject revealParticlePrefab;
    public AudioClip revealSoundEffect;
    public float screenshakeIntensity;
}
