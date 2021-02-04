using UnityEngine;

[System.Serializable]
public class Score
{
    public int score;
    public Sprite ScoreImage;
}

[CreateAssetMenu(fileName = "ScoreImage", menuName = "ScriptableObjects/ScoreImage", order = 1)]
public class ScoreImage : ScriptableObject
{
    [SerializeField]
    public Score[] ScoreItems;

}
