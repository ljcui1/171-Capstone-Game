using System.Collections;
using System.Collections.Generic;
using KevinCastejon.FiniteStateMachine;
using UnityEngine;

public class PlayerFSM : AbstractFiniteStateMachine
{
    public PlayerManager PlayMan { get; set; }
    private Rigidbody2D rb;

    [SerializeField]
    private float moveSpeed;

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
        public override void OnEnter() { }

        public override void OnUpdate()
        {
            if (GetStateMachine<PlayerFSM>().PlayMan.walking)
            {
                TransitionToState(PlayerState.WALK);
            }
            if (GetStateMachine<PlayerFSM>().PlayMan.playing)
            {
                TransitionToState(PlayerState.PLAY);
            }
            if (GetStateMachine<PlayerFSM>().PlayMan.talking)
            {
                TransitionToState(PlayerState.TALK);
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

            GetStateMachine<PlayerFSM>().moveInput.Normalize();

            //flip sprite
            if (GetStateMachine<PlayerFSM>().moveInput.x > 0)
            {
                GetStateMachine<PlayerFSM>().PlayMan.Player.transform.localRotation =
                    Quaternion.Euler(0, 0, 0);
            }
            else if (GetStateMachine<PlayerFSM>().moveInput.x < 0)
            {
                GetStateMachine<PlayerFSM>().PlayMan.Player.transform.localRotation =
                    Quaternion.Euler(0, 180, 0);
            }

            if (GetStateMachine<PlayerFSM>().PlayMan.idling)
            {
                TransitionToState(PlayerState.IDLE);
            }
            if (GetStateMachine<PlayerFSM>().PlayMan.playing)
            {
                TransitionToState(PlayerState.PLAY);
            }
            if (GetStateMachine<PlayerFSM>().PlayMan.talking)
            {
                TransitionToState(PlayerState.TALK);
            }
        }

        public override void OnFixedUpdate()
        {
            GetStateMachine<PlayerFSM>().rb.velocity =
                GetStateMachine<PlayerFSM>().moveInput * GetStateMachine<PlayerFSM>().moveSpeed;
        }

        public override void OnExit()
        {
            GetStateMachine<PlayerFSM>().PlayMan.walking = false;
        }
    }

    public class PlayState : AbstractState
    {
        public override void OnEnter() { }

        public override void OnUpdate()
        {
            if (GetStateMachine<PlayerFSM>().PlayMan.idling)
            {
                TransitionToState(PlayerState.IDLE);
            }
            if (GetStateMachine<PlayerFSM>().PlayMan.walking)
            {
                TransitionToState(PlayerState.WALK);
            }
            if (GetStateMachine<PlayerFSM>().PlayMan.talking)
            {
                TransitionToState(PlayerState.TALK);
            }
        }

        public override void OnFixedUpdate() { }

        public override void OnExit()
        {
            GetStateMachine<PlayerFSM>().PlayMan.playing = false;
        }
    }

    public class TalkState : AbstractState
    {
        public override void OnEnter()
        {
            Collider2D convo = GetStateMachine<PlayerFSM>().PlayMan.Player.talkTo;
        }

        public override void OnUpdate()
        {
            if (GetStateMachine<PlayerFSM>().PlayMan.idling)
            {
                TransitionToState(PlayerState.IDLE);
            }
            if (GetStateMachine<PlayerFSM>().PlayMan.playing)
            {
                TransitionToState(PlayerState.PLAY);
            }
            if (GetStateMachine<PlayerFSM>().PlayMan.walking)
            {
                TransitionToState(PlayerState.WALK);
            }
        }

        public override void OnFixedUpdate() { }

        public override void OnExit()
        {
            GetStateMachine<PlayerFSM>().PlayMan.talking = false;
        }
    }
}
