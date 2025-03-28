using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.FiniteStateMachine;
using Unity.VisualScripting;
public class CustomerStateMachine : AbstractFiniteStateMachine
{
    public CustomerManager Manager;
    public CustomerScript Script;
    public enum CustomerFSM
    {
        WALKIN,
        SIT,
        TALK,
        BOND,
        REJECT,
        ACCEPT,
        WALKOUT,
    }
    private void Awake()
    {
        Manager = transform.parent.gameObject.GetComponent<CustomerManager>();
        Init(
            CustomerFSM.WALKIN,
            AbstractState.Create<WalkInState, CustomerFSM>(CustomerFSM.WALKIN, this),
            AbstractState.Create<SitState, CustomerFSM>(CustomerFSM.SIT, this),
            AbstractState.Create<TalkState, CustomerFSM>(CustomerFSM.TALK, this),
            AbstractState.Create<BondState, CustomerFSM>(CustomerFSM.BOND, this),
            AbstractState.Create<RejectState, CustomerFSM>(CustomerFSM.REJECT, this),
            AbstractState.Create<AcceptState, CustomerFSM>(CustomerFSM.ACCEPT, this),
            AbstractState.Create<WalkOutState, CustomerFSM>(CustomerFSM.WALKOUT, this)
        );
    }
    public class WalkInState : AbstractState
    {
        public override void OnEnter()
        {
            //Start customer walking SFX when entering WalkIn state
            AudioManager.Instance.PlayWalkingSound(false);
        }
        public override void OnUpdate()
        {
            CustomerStateMachine FSM = GetStateMachine<CustomerStateMachine>();

            //animations
            Vector2 movement = FSM.Script.ai.desiredVelocity;
            if (movement.x > 0)
            {
                FSM.Script.anim.Play("right" + FSM.Script.num);
            }
            else if (movement.x < 0)
            {
                FSM.Script.anim.Play("left" + FSM.Script.num);
            }

            //sit
            if (FSM.Script.AtDestination() || FSM.Script.sit == true)
            {
                TransitionToState(CustomerFSM.SIT);
            }
        }
        public override void OnExit()
        {
            CustomerStateMachine FSM = GetStateMachine<CustomerStateMachine>();
            FSM.Script.walkin = false;
            FSM.Script.sit = true;

            //Stop walking SFX when reaching the destination
            AudioManager.Instance.StopWalkingSound(false);
        }
    }

    public class SitState : AbstractState
    {
        public override void OnEnter()
        {
            // animations
            CustomerStateMachine FSM = GetStateMachine<CustomerStateMachine>();
            FSM.Script.anim.Play("idle" + FSM.Script.num);
        }
        public override void OnUpdate()
        {
            CustomerStateMachine FSM = GetStateMachine<CustomerStateMachine>();

            if (FSM.Script.talk)
            {
                TransitionToState(CustomerFSM.TALK);
            }

            if (FSM.Script.walkout)
            {
                TransitionToState(CustomerFSM.WALKOUT);
            }
        }
        public override void OnExit()
        {
            GetStateMachine<CustomerStateMachine>().Script.sit = false;
        }
    }

    public class TalkState : AbstractState
    {
        public override void OnEnter()
        {
        }
        public override void OnUpdate()
        {
            CustomerStateMachine FSM = GetStateMachine<CustomerStateMachine>();

            if (FSM.Script.sit)
            {
                TransitionToState(CustomerFSM.SIT);
            }

            if (FSM.Script.walkout)
            {
                TransitionToState(CustomerFSM.WALKOUT);
            }
        }
        public override void OnExit()
        {
            GetStateMachine<CustomerStateMachine>().Script.talk = false;
        }
    }

    public class BondState : AbstractState
    {
        public override void OnEnter()
        {
        }
        public override void OnUpdate()
        {
            CustomerStateMachine FSM = GetStateMachine<CustomerStateMachine>();

            if (FSM.Script.reject)
            {
                TransitionToState(CustomerFSM.REJECT);
            }

            if (FSM.Script.bond)
            {
                TransitionToState(CustomerFSM.BOND);
            }
        }
        public override void OnExit()
        {
            GetStateMachine<CustomerStateMachine>().Script.bond = false;
        }
    }

    public class RejectState : AbstractState
    {
        public override void OnEnter()
        {
        }
        public override void OnUpdate()
        {
            CustomerStateMachine FSM = GetStateMachine<CustomerStateMachine>();

            if (FSM.Script.sit)
            {
                TransitionToState(CustomerFSM.SIT);
            }

            if (FSM.Script.walkout)
            {
                TransitionToState(CustomerFSM.WALKOUT);
            }
        }
        public override void OnExit()
        {
            GetStateMachine<CustomerStateMachine>().Script.reject = false;
        }
    }

    public class AcceptState : AbstractState
    {
        public override void OnEnter()
        {
            // animation
        }
        public override void OnUpdate()
        {
            CustomerStateMachine FSM = GetStateMachine<CustomerStateMachine>();

            if (FSM.Script.walkout)
            {
                TransitionToState(CustomerFSM.WALKOUT);
            }
        }
        public override void OnExit()
        {
            GetStateMachine<CustomerStateMachine>().Script.accept = false;
        }
    }

    public class WalkOutState : AbstractState
    {
        public override void OnEnter()
        {
            GetStateMachine<CustomerStateMachine>().Script.SetDestination(GetStateMachine<CustomerStateMachine>().Manager.entrance);
            //Start customer walking SFX when they leave
            AudioManager.Instance.PlayWalkingSound(false);
        }

        public override void OnUpdate()
        {
            CustomerStateMachine FSM = GetStateMachine<CustomerStateMachine>();

            //animations
            Vector2 movement = FSM.Script.ai.desiredVelocity;
            if (movement.x > 0)
            {
                FSM.Script.anim.Play("right" + FSM.Script.num);
            }
            else if (movement.x < 0)
            {
                FSM.Script.anim.Play("left" + FSM.Script.num);
            }

            if (FSM.Script.AtDestination())
            {
                FSM.Script.walkout = false;
                Debug.Log("Exiting");
                FSM.Script.Exit();

                // reset customer state to walkin
                TransitionToState(CustomerFSM.WALKIN);
                FSM.Script.walkin = true;
                FSM.Script.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        public override void OnExit()
        {
            //Stop walking SFX once they reach the exit

            AudioManager.Instance.StopWalkingSound(false);
        }
    }
}
