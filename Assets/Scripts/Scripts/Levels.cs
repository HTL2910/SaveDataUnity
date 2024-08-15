using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="World",menuName ="Level")]
public class Levels : ScriptableObject
{
    [Header("Board Dimensions")]
    public int width;
    public int height;

    [Header("Starting Tiles")]
    public TileType[] boardLayout;

    [Header("Avaiable Dots")]
    public GameObject[] dots;

    [Header("Score Goals")]
    public int[] scoreGoal;

    [Header("End Game Requirements")]
    public EndGameRequirements endGameRequirements;

    public BlankGoal[] levelGoals;
}
