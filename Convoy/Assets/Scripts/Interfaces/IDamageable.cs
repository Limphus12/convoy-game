using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.convoy
{
    public interface IDamageable
    {
        void Damage(int amount);

        void Death();

        bool IsDead();
    }
}