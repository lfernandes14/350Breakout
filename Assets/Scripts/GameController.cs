using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private BallBehaviour ball;

    // Start is called before the first frame update
    void Start()
    {
        DefinePlayerInput();
        CreateAllBricks();

        endGameText.gameObject.SetActive(false);

        ball = GameObject.FindObjectOfType<BallBehaviour>();
    }

    public void UpdateScore()
    {
        score += 100;
        scoreText.text = "Score: " + score.ToString();

        if( score >= 4000)
        {
            endGameText.text = "YOU WIN!!!";
            endGameText.gameObject.SetActive(true);
            ball.ResetBall();
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

    private void LaunchBall_started(InputAction.CallbackContext obj)
    {
        ball.LaunchTheBall();
    }

    private void Move_started(InputAction.CallbackContext obj)
    {
        print("Move Started");
        isPaddleMoving = true;
    }

    private void Move_canceled(InputAction.CallbackContext obj)
    {
        print("Move Cancelled");
        isPaddleMoving = false;
    }

    private void Quit_started(InputAction.CallbackContext obj)
    {
        print("Quit Cancelled");
    }

    private void Restart_started(InputAction.CallbackContext obj)
    {
        print("Restart Cancelled");
    }

    private void FixedUpdate()
    {
        if(isPaddleMoving)
        {
            //print("paddle moving");
            paddle.GetComponent<Rigidbody2D>().velocity = new Vector2(paddleSpeed * moveDirection, 0);            
        }
        else
        {
            //print("paddle NOT moving");
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
