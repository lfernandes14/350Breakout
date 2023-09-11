using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]private PlayerInput playerInput;
    private InputAction move;
    private InputAction restart;
    private InputAction quit;
    private InputAction launchBall;

    [SerializeField] private GameObject paddle;
    [SerializeField] private float paddleSpeed = 10;
    private bool isPaddleMoving;

    private float moveDirection;

    [SerializeField] private GameObject brick;

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private int score;

    [SerializeField] private TMP_Text endGameText;

    private BallBehaviour ballController;

    [SerializeField] private TMP_Text livesText;
    private int lives;

    [SerializeField] private TMP_Text restartText;
    [SerializeField] private TMP_Text launchText;

    // Start is called before the first frame update
    void Start()
    {
        DefinePlayerInput();
        CreateAllBricks();

        endGameText.gameObject.SetActive(false);

        ballController = GameObject.FindObjectOfType<BallBehaviour>();

        lives = 3;
        livesText.text = "Lives: " + lives.ToString();
        scoreText.text = "Score: " + score.ToString();

        restartText.gameObject.SetActive(false);
        launchText.gameObject.SetActive(true);
    }

    public void LoseALife()
    {
        lives--;
        livesText.text = "Lives: " + lives.ToString();

        if(lives == 0)
        {
            endGameText.text = "YOU HAVE FAILED!!";
            endGameText.gameObject.SetActive(true);
            ballController.StopBall();
            paddle.SetActive(false);
            restartText.gameObject.SetActive(true);
        }
        else
        {
            launchText.gameObject.SetActive(true);
        }
    }

    public void UpdateScore()
    {
        score += 100;
        scoreText.text = "Score: " + score.ToString();

        if( score >= 4000)
        {
            endGameText.text = "YOU WIN!!!";
            endGameText.gameObject.SetActive(true);
            ballController.StopBall();
            paddle.SetActive(false);
            restartText.gameObject.SetActive(true);
        }
    }

    private void CreateAllBricks()
    {
        Vector2 brickPos = new Vector2(-9, 5f);

        for (int j = 0; j < 4; j++)
        {
            brickPos.y -= 1;
            brickPos.x = -9;

            for (int i = 0; i < 10; i++)
            {
                brickPos.x += 1.6f;        //x = x + 1
                Instantiate(brick, brickPos, Quaternion.identity);
            }
        }
    }

    private void DefinePlayerInput()
    {
        playerInput.currentActionMap.Enable();

        move = playerInput.currentActionMap.FindAction("Move");
        restart = playerInput.currentActionMap.FindAction("RestartGame");
        quit = playerInput.currentActionMap.FindAction("QuitGame");
        launchBall = playerInput.currentActionMap.FindAction("LaunchBall");

        move.started += Move_started;
        restart.performed += Restart_started;
        quit.performed += Quit_started;
        launchBall.started += LaunchBall_started;

        move.canceled += Move_canceled;
    }

    private void OnDestroy()
    {
        move.started -= Move_started;
        restart.performed -= Restart_started;
        quit.performed -= Quit_started;
        launchBall.started -= LaunchBall_started;
        move.canceled -= Move_canceled;
    }

    private void LaunchBall_started(InputAction.CallbackContext obj)
    {
        ballController.LaunchTheBall();
        launchText.gameObject.SetActive(false);
    }

    private void Move_started(InputAction.CallbackContext obj)
    {
        isPaddleMoving = true;
    }

    private void Move_canceled(InputAction.CallbackContext obj)
    {
        isPaddleMoving = false;
    }

    private void Quit_started(InputAction.CallbackContext obj)
    {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    private void Restart_started(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene(0);
        restartText.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if(isPaddleMoving)
        {
            
            paddle.GetComponent<Rigidbody2D>().velocity = new Vector2(paddleSpeed * moveDirection, 0);            
        }
        else
        {
            paddle.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    private void Update()
    {
        if(isPaddleMoving)
        {
            moveDirection = move.ReadValue<float>();
        }
    }
}
