using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public List<int> piecesInStates = new List<int>();
   // public List<Destructible> drones = new List<Destructible>();
    int currentState = 0;

    public Animator anim;

    public void PieceDestroyed(Destructible piece)
    {
        piecesInStates[currentState] -= 1;
        if (piecesInStates[currentState] == 0)
        {
            if (piecesInStates.Count > currentState + 1)
            {
                NextState();
            }
            else
                BossDefeated();
        }
    }

    void NextState()
    {
        currentState += 1;
        anim.SetInteger("BossState", currentState);
    }

    void BossDefeated()
    {
        print("boss defeated");
    }
}