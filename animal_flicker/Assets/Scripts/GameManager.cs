using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public FlickScript flickScript;
    public Transform launchStartPosition;
    public TextMeshProUGUI gameStatusText;
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;

    private int currentPlayer = 1;
    private int round = 1;
    private int maxRounds = 3;

    private float player1Score = 0f;
    private float player2Score = 0f;

    public bool isLaunching = false;

    void Start()
    {
        UpdateUI();
        ResetObjectPosition();
    }

    void Update()
    {
        Debug.Log("Vel: " + flickScript.GetComponent<Rigidbody>().velocity.magnitude);
        if (isLaunching) return;

        // Only start counting when the object has been launched
        if (flickScript != null && flickScript.IsLaunched)
        {
            // Check if the object velocity is below threshold and if it's time to wait
            if (flickScript.GetComponent<Rigidbody>().velocity.magnitude < 0.1f || flickScript.transform.position.y < 0)
            {
                StartCoroutine(WaitForObjectToStop());
            }
        }
    }

    // Coroutine to wait for the object to stop moving
    private IEnumerator WaitForObjectToStop()
    {
        // Wait for 2 seconds to ensure the object has stopped moving
        yield return new WaitForSeconds(2f);

        // Now check if the object is truly stopped
        if (flickScript.GetComponent<Rigidbody>().velocity.magnitude < 0.1f)
        {
            if (flickScript.transform.position.y < 0)
            {
                AddScore(0); // Out of bounds
            }
            else
            {
                float distance = Vector3.Distance(flickScript.transform.position, launchStartPosition.position);
                Debug.Log("Dist: " + distance);
                AddScore(distance);
            }

            NextTurn();
        }
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
        isLaunching = true;

        Rigidbody rb = flickScript.GetComponent<Rigidbody>();

        // Reset position and velocity
        flickScript.transform.position = launchStartPosition.position;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        flickScript.ResetLaunchFlag(); // Reset the launch flag in FlickScript
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
