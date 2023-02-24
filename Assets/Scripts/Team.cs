using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    public static string[] teams = {"player", "enemy"};
    public int team;

    public int getTeam() {
        return team;
    }
    public string getTeamName() {
        return Team.teams[team];
    }
    public void setTeam(int t) {
        team = t;
    }
}
