using System.Collections;
using System.Collections.Generic;
using KevinCastejon.FiniteStateMachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerFSM : AbstractFiniteStateMachine
{
    public PlayerManager PlayMan { get; set; }
    private Rigidbody2D rb;

    // adding a Unity event to increment 30 minutes after minigame completion

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    public UnityEvent onMinigameCompletion;

    //float speedX, speedY = 0;
    private Vector2 moveInput;

    public enum PlayerState
    {
        IDLE,
        WALK,
        PLAY,
        TALK,
    }

    private void Awake()
    {
        Init(
            PlayerState.IDLE,
            AbstractState.Create<IdleState, PlayerState>(PlayerState.IDLE, this),
            AbstractState.Create<WalkState, PlayerState>(PlayerState.WALK, this),
            AbstractState.Create<PlayState, PlayerState>(PlayerState.PLAY, this),
            AbstractState.Create<TalkState, PlayerState>(PlayerState.TALK, this)
        );

        PlayMan = transform.GetComponent<PlayerManager>();
        rb = PlayMan.Player.GetComponent<Rigidbody2D>();
    }

    public class IdleState : AbstractState
    {
        public override void OnEnter()
        {
            GetStateMachine<PlayerFSM>().PlayMan.Player.anims.Play("Idle");
        }

        public override void OnUpdate()
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (GetStateMachine<PlayerFSM>().PlayMan.Player.inRange)
                {
                    GetStateMachine<PlayerFSM>().PlayMan.talking = true;
                    TransitionToState(PlayerState.TALK);
                }
                else if (GetStateMachine<PlayerFSM>().PlayMan.Player.startPlay)
                {
                    GetStateMachine<PlayerFSM>().PlayMan.playing = true;
                    TransitionToState(PlayerState.PLAY);
                }
            }

            if (
                GetStateMachine<PlayerFSM>().PlayMan.joyIn
                && !GetStateMachine<PlayerFSM>().PlayMan.playing
                && !GetStateMachine<PlayerFSM>().PlayMan.talking
            )
            {
                GetStateMachine<PlayerFSM>().PlayMan.walking = true;
                TransitionToState(PlayerState.WALK);
            }
        }

        public override void OnFixedUpdate() { }

        public override void OnExit()
        {
            GetStateMachine<PlayerFSM>().PlayMan.idling = false;
        }
    }

    public class WalkState : AbstractState
    {
        public override void OnEnter() { }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.E) && GetStateMachine<PlayerFSM>().PlayMan.Player.inRange)
            {
                GetStateMachine<PlayerFSM>().PlayMan.talking = true;
                TransitionToState(PlayerState.TALK);
            }

            if (GetStateMachine<PlayerFSM>().PlayMan.Player.startPlay && Input.GetKey(KeyCode.Q))
            {
                GetStateMachine<PlayerFSM>().PlayMan.playing = true;
                TransitionToState(PlayerState.PLAY);
            }

            if (!GetStateMachine<PlayerFSM>().PlayMan.joyIn)
            {
                GetStateMachine<PlayerFSM>().PlayMan.idling = true;
                TransitionToState(PlayerState.IDLE);
            }
            /*Debug.Log("Vertical" + Input.GetAxis("Vertical"));
            // movement
            speedX = 0f;
            speedY = 0f;

            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                speedX = Input.GetAxisRaw("Horizontal") * Player.moveSpeed;
            }
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                speedY = Input.GetAxisRaw("Vertical") * Player.moveSpeed;
            }

            Debug.Log("speedX" + speedX + ", speedY" + speedY);
            rb.velocity = new Vector2(speedX, speedY).normalized * Player.moveSpeed;
            */

            //movement 2
            GetStateMachine<PlayerFSM>().moveInput.x = Input.GetAxisRaw("Horizontal");
            GetStateMachine<PlayerFSM>().moveInput.y = Input.GetAxisRaw("Vertical");
            // Debug.Log(GetStateMachine<PlayerFSM>().moveInput.x);
            // Debug.Log(GetStateMachine<PlayerFSM>().moveInput.y);

            GetStateMachine<PlayerFSM>()
                .moveInput.Normalize();

            //flip sprite
            if (GetStateMachine<PlayerFSM>().moveInput.x > 0)
            {
                //GetStateMachine<PlayerFSM>().PlayMan.Player.transform.localRotation =
                //    Quaternion.Euler(0, 0, 0);
                GetStateMachine<PlayerFSM>()
                    .PlayMan.Player.anims.Play("RightWalk");
            }
            else if (GetStateMachine<PlayerFSM>().moveInput.x < 0)
            {
                //GetStateMachine<PlayerFSM>().PlayMan.Player.transform.localRotation =
                //    Quaternion.Euler(0, 180, 0);
                GetStateMachine<PlayerFSM>()
                    .PlayMan.Player.anims.Play("LeftWalk");
            }
            if (
                GetStateMachine<PlayerFSM>().moveInput.y > 0
                && GetStateMachine<PlayerFSM>().moveInput.x == 0
            )
            {
                GetStateMachine<PlayerFSM>().PlayMan.Player.anims.Play("BackWalk");
            }
            else if (
                GetStateMachine<PlayerFSM>().moveInput.y < 0
                && GetStateMachine<PlayerFSM>().moveInput.x == 0
            )
            {
                GetStateMachine<PlayerFSM>().PlayMan.Player.anims.Play("FrontWalk");
            }
        }

        public override void OnFixedUpdate()
        {
            GetStateMachine<PlayerFSM>().rb.velocity =
                GetStateMachine<PlayerFSM>().moveInput * GetStateMachine<PlayerFSM>().moveSpeed;
            // Debug.Log(GetStateMachine<PlayerFSM>().rb.velocity);
        }

        public override void OnExit()
        {
            GetStateMachine<PlayerFSM>().PlayMan.walking = false;
        }
    }

    public class PlayState : AbstractState
    {
        public override void OnEnter()
        {
            Debug.Log(GetStateMachine<PlayerFSM>().PlayMan.Player.talkTo);
            if (GetStateMachine<PlayerFSM>().PlayMan.Player.talkTo.tag == "Minigame")
            {
                GetStateMachine<PlayerFSM>()
                    .PlayMan.MiniMan.StartMinigame(
                        GetStateMachine<PlayerFSM>()
                            .PlayMan.Player.talkTo.GetComponent<BaseMinigame>()
                            .attribute
                    );
            }
            else
            {
                Debug.Log("Entered play state with no minigame to play");
                TransitionToState(PlayerState.IDLE);
            }
        }

        public override void OnUpdate()
        {
            if (GetStateMachine<PlayerFSM>().PlayMan.idling || Input.GetKeyDown(KeyCode.Escape))
            {
                TransitionToState(PlayerState.IDLE);
            }
        }

        public override void OnFixedUpdate() { }

        public override void OnExit()
        {
            GetStateMachine<PlayerFSM>().PlayMan.MiniMan.StopMinigame();
            GetStateMachine<PlayerFSM>().PlayMan.Player.startPlay = false;
            GetStateMachine<PlayerFSM>().PlayMan.playing = false;
            GetStateMachine<PlayerFSM>().onMinigameCompletion.Invoke();
        }
    }

    public class TalkState : AbstractState
    {
        public override void OnEnter()
        {
            // Time.timeScale = 0f;
        }

        public override void OnUpdate()
        {
            if (GetStateMachine<PlayerFSM>().PlayMan.idling)
            {
                TransitionToState(PlayerState.IDLE);
            }
        }

        public override void OnFixedUpdate() { }

        public override void OnExit()
        {
            // Time.timeScale = 1f;
            GetStateMachine<PlayerFSM>().PlayMan.talking = false;
        }
    }
}
