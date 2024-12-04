using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public FlickScript flickScript;
    public Transform launchStartPosition;
    public TextMeshProUGUI gameStatusText;
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;

    //public AudioClip backgroundMusic;
    public AudioClip fallOffSound;

    private AudioSource audioSource;  // AudioSource for playing sounds

    private int currentPlayer = 1;
    private int round = 1;
    private int maxRounds = 3;

    private float player1Score = 0f;
    private float player2Score = 0f;

    public bool isLaunching = false;

    void Start()
    {
        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();

        // Play background music on loop
        //if (backgroundMusic != null)
        //{
        //    audioSource.clip = backgroundMusic;
        //    audioSource.loop = true;
        //    audioSource.Play();
        //}

        UpdateUI();
        ResetObjectPosition();
    }

    void Update()
    {
        if (isLaunching) return;

        Rigidbody rb = flickScript.GetComponent<Rigidbody>();
        if (flickScript.IsLaunched)
        {
            if (flickScript.transform.position.y < -5)
            {
                isLaunching = true;
                StartCoroutine(HandleOutofBounds());
            }
            else if (rb.velocity.magnitude < 0.1f && flickScript.CheckIfGrounded())
            {
                isLaunching = true;
                StartCoroutine(HandleTurnEnd());
            }
        }
    }

    IEnumerator HandleTurnEnd()
    {
        yield return new WaitForSeconds(0.5f);

        float distance = Vector3.Distance(flickScript.transform.position, launchStartPosition.position);
        AddScore(distance);

        NextTurn();
    }

    IEnumerator HandleOutofBounds()
    {
        PlayFallOffSound();  // Play fall off sound
        yield return new WaitForSeconds(0.5f);

        Debug.Log("Out of Bounds");
        AddScore(0);

        NextTurn();
    }

    void AddScore(float score)
    {
        if (currentPlayer == 1)
        {
            player1Score += score;
        }
        else
        {
            player2Score += score;
        }
    }

    void NextTurn()
    {
        if (currentPlayer == 1)
        {
            currentPlayer = 2;
        }
        else
        {
            currentPlayer = 1;
            round++;
        }

        if (round > maxRounds)
        {
            UpdateUI();
            EndGame();
        }
        else
        {
            ResetObjectPosition();
            UpdateUI();
        }
    }

    void ResetObjectPosition()
    {
        Debug.Log("ResetObjectPosition called");
        isLaunching = true;

        Rigidbody rb = flickScript.GetComponent<Rigidbody>();

        // Reset position, rotation, and physics
        flickScript.transform.position = launchStartPosition.position;
        flickScript.transform.rotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep(); // Ensures Rigidbody stops simulating physics

        Debug.Log($"Position set to: {launchStartPosition.position}");

        // Reset FlickScript state (ensure these methods don't reset position)
        flickScript.ResetLaunchFlag();
        flickScript.ResetFlickState();

        isLaunching = false;
    }


    void PlayFallOffSound()
    {
        if (fallOffSound != null)
        {
            audioSource.PlayOneShot(fallOffSound);
        }
    }

    void UpdateUI()
    {
        gameStatusText.text = $"Player {currentPlayer}'s Turn - Round {round}";
        player1ScoreText.text = $"Player 1 Score: {player1Score:F1}";
        player2ScoreText.text = $"Player 2 Score: {player2Score:F1}";
    }

    void EndGame()
    {
        string winner = player1Score > player2Score ? "Player 1 Wins!" :
                        player2Score > player1Score ? "Player 2 Wins!" : "It's a Tie!";
        gameStatusText.text = $"Game Over! {winner}";
    }
}
