using UnityEngine;
using UnityEngine.AI;

namespace com.lineact.lit.FSM
{
    [CreateAssetMenu(menuName = "LIT/FSM/Transitions/ReachedWaypointNavMeshTransition")]
    public class ReachedWaypointNavMeshTransition : Transition
    {
        public override bool Decide(BaseStateMachine stateMachine)
        {
            // telling that agent has reached the goal or has no path to reach the point.
            var navMeshAgent = stateMachine.GetComponent<NavMeshAgent>();
            if (!navMeshAgent.pathPending && navMeshAgent.isActiveAndEnabled)
            {
                if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                {
                    if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}