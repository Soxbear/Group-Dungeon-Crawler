using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{

    [Header("Stats")]
    [Tooltip("The Player's current health")]
    public int Health;

    
    [Header("Basic Settings")]
    [Tooltip("The Player's maximumn health")]
    public int MaxHealth = 100;
    [Tooltip("The Player's speed")]
    public float Speed = 2.5f;

    [Header("Advanced Settings")]
    [Tooltip("The minimumn time (in seconds) between eye blinks")]
    public int MinimumnBlinkTime = 3;
    [Tooltip("The maximumn time (in seconds) between eye blinks")]
    public int MaximumnBlinkTime = 7;

    //Ignore
    private Vector2 MovementInput; //Store the movement inputs
    private Vector2 LastPos; //Store the position of the player last frame
    private int TimeSinceLastBlink; //Store the time since last blink

    void Start()
    {
        LastPos = new Vector2(transform.position.x, transform.position.y); //Store the position of the player
    }

    void Update()
    {
        //Player Movement
        MovementInput.x = Input.GetAxis("Horizontal"); //Store the Players horizontal input
        MovementInput.y = Input.GetAxis("Vertical"); //Store the Players vertical input
        MovementInput *= Time.deltaTime; //Adjust the movement to be consistent every frame
        GetComponent<Rigidbody2D>().MovePosition(new Vector2(transform.position.x, transform.position.y) + MovementInput * Speed); //Move the Player
        GetComponent<Animator>().SetFloat("X", MovementInput.x); //Feed the animator the Player's horizontal movement
        GetComponent<Animator>().SetFloat("Y", MovementInput.y); //Feed the animator the Player's vertical movement
        if (Mathf.Abs(MovementInput.x) > 0 || Mathf.Abs(MovementInput.y) > 0) //If the player is moving
            GetComponent<Animator>().SetFloat("Speed", 1); //Feed the animator the Player's movement
        else //If the player is not moving
            GetComponent<Animator>().SetFloat("Speed", 0); //Feed the animator the Player's movement
        LastPos = new Vector2(transform.position.x, transform.position.y); //Store the position of the player
        
        //Input Mouse Pos
        GetComponent<Animator>().SetFloat("MouseX", Input.mousePosition.x - (0.5f * Screen.width)); //Input Mouse X
        GetComponent<Animator>().SetFloat("MouseY", Input.mousePosition.y - (0.5f * Screen.height)); //Input Mouse Y
    }

    //A function to let things deal damage to the Player
    public void TakeDamage(int Amount)
    {
        Health -= Amount; //Remove the damage from the health
        if (Health <= 0) //Check if the Player is death
        {
            //Enact death punishment
        }
    }


    //A few notes about the ChangeEyes() function
    //
    //It is called from the animations using events at the end of look anims
    //Each if statement that looks at the random number has the chance that it is true next to it
    //Some of the look statements are missing a direction, as it is the direction it was looking in and leaving out the code doesn't change the result
    public void ChangeEyes()
    {
        float CurrentDirection = GetComponent<Animator>().GetFloat("Look"); //Get the direction the eyes are currently facing
        if (TimeSinceLastBlink < MinimumnBlinkTime) //If the Player just blinked
        {
            if (CurrentDirection == 0.3 || CurrentDirection == 1) //If the Player is looking forward or just blinked
            {
                float rand = Random.Range(0f, 6f); //Get random number
                if (rand >= 0 && rand <= 4) // 2/3 chance
                    GetComponent<Animator>().SetFloat("Look", 0.3f); //Look forward
                else if (rand > 4 && rand <= 5) // 1/6 chance
                    GetComponent<Animator>().SetFloat("Look", 0f); //Look right
                else //Remaining 1/6
                    GetComponent<Animator>().SetFloat("Look", 0.7f); //Loof left
            }
            else if (CurrentDirection == 0) //If the Player is looking right
            {
                float rand = Random.Range(0f, 5f); //Get random number
                if (rand > 2 && rand <= 4) // 2/5 chance
                    GetComponent<Animator>().SetFloat("Look", 0.3f); //Look forward
                else if (rand > 4 && rand <= 5) // 1/5 chance
                    GetComponent<Animator>().SetFloat("Look", 0.7f); //Look left
            }
            else //If the Player is looking left
            {
                float rand = Random.Range(0f, 5f); //Get a random number
                if (rand > 2 && rand <= 3) // 1/5 chance
                    GetComponent<Animator>().SetFloat("Look", 0f); //Look right
                else if (rand > 3 && rand <= 5) // 2/5 chance
                    GetComponent<Animator>().SetFloat("Look", 0.3f); //Look forward
            }
            TimeSinceLastBlink++; //Increase the variable that stores the time since last blink
        }
        else if (TimeSinceLastBlink >= MinimumnBlinkTime && TimeSinceLastBlink < MaximumnBlinkTime) //If the Player hasn't blinked in a few seconds
        {
            if (CurrentDirection == 0.3) //If the Player is looking forward
            {
                float rand = Random.Range(0f, 6f); //Get a random number
                if (rand > 3 && rand <= 4) // 1/6 chance
                    GetComponent<Animator>().SetFloat("Look", 0f); //Look right
                else if (rand > 4 && rand <= 5) // 1/6 chance
                    GetComponent<Animator>().SetFloat("Look", 0.7f); //Look left
                else if (rand > 6) // 1/6 chance
                    Blink(); //Blink
            }
            else if (CurrentDirection == 0) //If the Player is looking right
            {
                float rand = Random.Range(0f, 6f); //Get a random number
                if (rand > 2 && rand <= 4) // 1/3 chance
                    GetComponent<Animator>().SetFloat("Look", 0.3f); //Look forward
                else if (rand > 4 && rand <= 5) // 1/6 chance
                    GetComponent<Animator>().SetFloat("Look", 0.7f); //Look left
                else if (rand > 5) // 1/6 chance
                    Blink(); //Blink
            }
            else //If the Player is looking left
            {
                float rand = Random.Range(0f, 6f); //Get a random number
                if (rand > 2 && rand <= 3) // 1/6 chance
                    GetComponent<Animator>().SetFloat("Look", 0f); //Look right
                else if (rand > 3 && rand <= 5) // 1/3 chance
                    GetComponent<Animator>().SetFloat("Look", 0.3f); //Look forward
                else if (rand > 5) // 1/6 chance
                    Blink(); //Blink
            }
            if (TimeSinceLastBlink != 0) //If the Player didn't blink
                TimeSinceLastBlink++; //Increase the time since last blink
        }
        else //If the Player hasn't blinked in a long time
            Blink(); //Blink
    }

    //This is the function called by ChangeEyes() to make the Player blink
    void Blink()
    {
        GetComponent<Animator>().SetFloat("Look", 1); //Tell the animator to make the Player blink
        TimeSinceLastBlink = 0; //Tell ChangeEyes() that the Player blinked
    }
}
