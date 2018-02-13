using UnityEngine;
using System.Collections;

public class BeltScript : MonoBehaviour {

    //-1 = left, 0 = stationary, 1 = right

    public BombScript bomb;
    public ButtonScript blueButton, redButton;
    public GameObject beltObject;
    public Material blueArrows, redArrows, neutralArrows;

    //Master script
    GameScript mainScript;

    float leftSide, rightSide;

	// Use this for initialization
	void Start () {
        beltObject.GetComponent<Renderer>().material = neutralArrows;
        
        leftSide = beltObject.transform.position.x - (beltObject.GetComponent<Renderer>().bounds.size.x / 2);
        rightSide = beltObject.transform.position.x + (beltObject.GetComponent<Renderer>().bounds.size.x / 2);


        float beltTextureScale = beltObject.GetComponent<Renderer>().bounds.size.x / 3;

        blueArrows.mainTextureScale = new Vector2(beltTextureScale, 1);
        redArrows.mainTextureScale = new Vector2(beltTextureScale, 1);
        neutralArrows.mainTextureScale = new Vector2(beltTextureScale, 1);
	}
	
	// Update is called once per frame
	void Update () {

        if (bomb.Position < leftSide)
        {
            bomb.explode();
            mainScript.damage(true);
            beltObject.GetComponent<Renderer>().material = neutralArrows;
        }
        else if (bomb.Position > rightSide)
        {
            bomb.explode();
            mainScript.damage(false);
            beltObject.GetComponent<Renderer>().material = neutralArrows;
        }

        beltObject.GetComponent<Renderer>().material.mainTextureOffset += new Vector2(bomb.Direction * bomb.CurrentSpeed * 0.00575f, 0);

        if (bomb.Direction == -1)
            redButton.glowToggle(false);
        else
            redButton.glowToggle(true);

        if (bomb.Direction == 1)
            blueButton.glowToggle(false);
        else
            blueButton.glowToggle(true);

	}

    public void setup(GameScript game)
    {
        mainScript = game;
    }

    public void stop()
    {
		if (bomb.currentSpeed != 0) {
			StartCoroutine (bomb.stop ());
		}
        beltObject.GetComponent<Renderer>().material = neutralArrows;
    }

    public void pushButton(bool playerID)
    {
        if (bomb.Direction == 0)
            bomb.startBomb();

        if (playerID == true)
        {
            bomb.Direction = 1;
            blueButton.resetCooldown();
			redButton.resetCooldown();
            beltObject.GetComponent<Renderer>().material = blueArrows;
        }
        else
        {
            bomb.Direction = -1;
			blueButton.resetCooldown();
			redButton.resetCooldown();			
			beltObject.GetComponent<Renderer>().material = redArrows;

        }
    }

    public bool attemptPush(bool playerID)
    {
        if (playerID == true)
        {
			return blueButton.CanBePressed && getDirection(playerID) != 1 && bomb.transform.position.y == bomb.initialPosition.y;
        }
        else
			return redButton.CanBePressed && getDirection(playerID) != 1 && bomb.transform.position.y == bomb.initialPosition.y;
    }

    #region Belt Info
    public int getDirection(bool playerID)
    {
        if (bomb.Direction == 0)
            return 0;

        //We assume that BLUE is on the LEFT and RED is on the RIGHT
        if (playerID == true)
        {
            return bomb.Direction;
        }
        else
        {
            return -bomb.Direction;
        }
    }

	/// <summary>
	/// Calculates the distance from the player's side to the belt's bomb.
	/// Value is infinity if the bomb has not spawned yet.
	/// </summary>
	/// <returns>The bomb distance.</returns>
	/// <param name="playerID">If set to <c>true</c> player I.</param>
    public float getBombDistance(bool playerID)
    {
        //We assume that BLUE is on the LEFT and RED is on the RIGHT
        if (playerID == true)
        {
			if (bomb.transform.position.y < bomb.initialPosition.y) {
				return Mathf.Infinity;
			} else {
				return bomb.Position - leftSide;
			}
        }
        else
        {
			if (bomb.transform.position.y < bomb.initialPosition.y) {
				return Mathf.Infinity;
			} else {
				return rightSide - bomb.Position;
			}
        }
    }

    public float getCoolDown(bool playerID)
    {
        //We assume that BLUE is on the LEFT and RED is on the RIGHT
        if (playerID == true)
        {
            return blueButton.CurrentCooldown;
        }
        else
        {
            return redButton.CurrentCooldown;
        }
    }

    public float BombSpeed
    {
        get { return bomb.CurrentSpeed; }
    }

    public float Position
    {
        get { return transform.position.z; }
    }

    #endregion
}
