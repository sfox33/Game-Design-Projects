using UnityEngine;
using System.Collections;

public class AI_SeanFox : MonoBehaviour {

    public CharacterScript mainScript;

    public float[] bombSpeeds;
    public float[] buttonCooldowns;
    public float playerSpeed;
    public int[] beltDirections;
    public float[] buttonLocations;
	public float[] bombDist;
	private bool isRed;
	//private float[] heuristicVals;
	private float currHeuristic;
	private float bestHeuristic;
	private int minIndex;
	private float pos;
//	private bool hasDestination;
	private float h;
	private float agentTime;
	private float bombTime;
	private int frames;

	// Use this for initialization
	void Start () {
        mainScript = GetComponent<CharacterScript>();

		frames = 0;

        if (mainScript == null)
        {
            print("No CharacterScript found on " + gameObject.name);
            this.enabled = false;
        }

        buttonLocations = mainScript.getButtonLocations();

        playerSpeed = mainScript.getPlayerSpeed();

		pos = mainScript.getCharacterLocation ();
		mainScript.push ();
	}

	// Update is called once per frame
	void Update () {
		pos = mainScript.getCharacterLocation ();
        buttonCooldowns = mainScript.getButtonCooldowns();
        beltDirections = mainScript.getBeltDirections();
		bombDist = mainScript.getBombDistances ();
		bombSpeeds = mainScript.getBombSpeeds ();
		frames++;        
		if (frames >= 100) {
			frames = 0;
		}
        //Your AI code goes here
		if (frames % 5 == 0) {
			bestHeuristic = Mathf.Infinity;
			minIndex = -1;
			//Calculates the heuristic values for each bomb;  chooses the bomb with the lowest heuristic
			for (int i = 0; i < beltDirections.Length; ++i) {
				currHeuristic = heuristic (pos, buttonLocations [i], bombDist [i], bombSpeeds [i], beltDirections [i], buttonCooldowns [i]);
				if (currHeuristic >= 0 && currHeuristic < bestHeuristic && beltDirections [i] <= 0) {
					bestHeuristic = currHeuristic;
					minIndex = i;
				}
			}
		}
 
		if (buttonLocations[minIndex] > pos) {
			mainScript.moveUp ();
		} else {
			mainScript.moveDown ();
		}
		if (Mathf.Abs(buttonLocations [minIndex] - pos) < 1.0f) {
			mainScript.push ();
		}
	}

	/*
	 * Calculates a heuristic value for the bomb that it is given.  Features include the amount of time
	 * it would take for the agent to reach a button, the amount of time it would take for the bomb
	 * to get to the agent's side, the speed of the bomb, and the distance of the bomb from the agent's side
	 */
	private float heuristic(float position, float goal, float bombDist, float bombSpeed, int bombDirection, float cooldown) {
		h = 0.0f;
		agentTime = (Mathf.Abs(goal - position)) / (5.0f);		//Roughly the time needed to get to destination

		if (bombSpeed == 0) {			//The length of time it will take for bomb to reach agent's side is a feature
			bombTime = Mathf.Infinity;
		} else {
			bombTime = bombDist / bombSpeed;
		}

		if (agentTime < bombTime) {				//If agent can reach button in time...
			h += agentTime;			//agentTime is a feature
			if (bombTime == Mathf.Infinity)
				h += 10;
			else
				h += bombTime;
		} else {				//If agent has no chance of hitting the button
			return Mathf.Infinity;
		}

		if (h < cooldown) {
			h += 10* (cooldown - agentTime);		//De-prioritize buttons with cooldown
		}
		if (bombSpeed != 0f && bombDirection < 0) {
			if (bombDist >= 1) {
				h += 5f * 1 / bombDist;	//Proritize the lack of distance
			} else {
				h += 0.5f * (bombDist);
			}
		}
		if (bombSpeed != 0) {	//Use the speed of the bomb as a feature
			h += 5f * 1/(bombSpeed);	//Faster the speed, the higher priority the bomb
		}
		return h;
	}
}
