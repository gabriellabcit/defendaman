﻿using UnityEngine;
using System.Collections;
using System;

public class Building:MonoBehaviour {
	
	public enum BuildingType{Empty,Armory,Wall,Watchtower,Turret, Alchemist};

	public BuildingType type;

	public float health = 100;

	public int team;
	public int collidercounter=0;
	public Sprite allyBuilding;
	public Sprite enemyBuilding;

	public float ConstructionTime = 2f;

	SpriteRenderer spriteRenderer;

	[HideInInspector]
	public bool placing = true;
	[HideInInspector]
	public bool placeble;
	[HideInInspector]
	public bool constructed = false;

	// Use this for initialization
	void Start () 
	{
		collidercounter=0;
		if(!placing)
			//gameObject.GetComponent<Animator>().SetTrigger("Create");
			StartCoroutine(Construct());

		placeble = true;
		if(GameData.MyPlayer.TeamID == team)
			gameObject.GetComponent<SpriteRenderer>().sprite = allyBuilding;
		else
			gameObject.GetComponent<SpriteRenderer>().sprite = enemyBuilding;

    }

	IEnumerator Construct()
	{
		float elapsedTime = 0.0f;
		Vector3 startingScale = new Vector3(0, 0, 0); // have a startingRotation as well
		Vector3 targetScale = transform.localScale;

		while (elapsedTime < ConstructionTime) 
		{
			elapsedTime += Time.deltaTime; // <- move elapsedTime increment here
			// Scale
			transform.localScale = Vector3.Slerp(startingScale, targetScale, (elapsedTime / ConstructionTime));
			yield return new WaitForEndOfFrame ();
		}
		constructed = true;
	}

	/*----------------------------------------------------------------------------
    --	Called when an collidable object enters the bounding box
    --
    --	Interface: 	void OnTriggerEnter2D(Collider2D other)
    --					-Collider2D other: Other collider that entered 
    --
    --	programmer: Jerry Jia, Thomas Yu
    --	@return: void
	------------------------------------------------------------------------------*/
	void OnTriggerEnter2D(Collider2D other) 
	{
		print ("Enter tag is: " + other.gameObject.tag);
		if(other.gameObject.tag == "Bullet")
			return;
		if(placing && other.gameObject.tag!="Untagged" )
		{
			collidercounter++;

			print ("Being increased" + collidercounter);
			placeble = false;

		}
		if(health<=0)
			Destroy(gameObject);
		var attack = other.gameObject.GetComponent<Trigger>();
		if (attack != null)
		{
			if (attack.teamID == team)
				return;
			float damage = other.GetComponent<Trigger>().damage;
			health -= damage;
		}
		notifydeath();
	}

	/*----------------------------------------------------------------------------
    --	Called when an collidable object leaves the bounding box of it's BoxCollider2D
    --
    --	Interface: 	void OnTriggerExit2D(Collider2D other)
    --					-Collider2D other: Other collider that left 
    --
    --	programmer: Jerry Jia, Thomas Yu
    --	@return: void
	------------------------------------------------------------------------------*/
	void OnTriggerExit2D(Collider2D other)
	{
		print ("Enter tag is: " + other.gameObject.tag);
		if(placing && other.gameObject.tag!="Untagged"  )
		{
			collidercounter--;
			placeble = true;
			print (collidercounter);
		}
	}

	public void notifycreation(){
		//????
	}
	public void	notifydeath()
	{
		//?
	}
	
}
