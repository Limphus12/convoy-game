using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;

namespace com.limphus.convoy
{
    public class LevelCompleteTriggerEvent : TriggerEvent
    {
        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);

            if (triggered)
            {
                GameManager.ChangeGameState(GameState.Complete);
            }
        }
    }
}