using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    //[SerializeField] private TMP_Text m_Text;
    [SerializeField] private TextMeshProUGUI scoreText;
    internal int score;

    // Update is called once per frame
    void Update()
    {
        int initialScore = 0;
        int finalScore = initialScore + score;
        scoreText.text = "Score: " + finalScore.ToString();
    }
}
