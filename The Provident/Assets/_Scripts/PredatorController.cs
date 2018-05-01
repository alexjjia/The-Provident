using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class PredatorController : EntityController {
//	private float attackZone = 0.9f; // radius at which the entity can "eat" something.

	protected override void Start()
	{

		base.Start ();
		//Debug.Log ("food length is: " + food.Length);
		//food = GameObject.FindGameObjectsWithTag ("Prey");
		behaviorRate = 80;
	}

	protected override void Update () 
	{
		base.Update ();
//		if (food == null) {
//			Debug.Log ("food is now of size: " + food.Length);
//		}
	}
	
	protected override void FixedUpdate()
	{
//		if(Input.GetKey(KeyCode.Space)) //USED FOR TESTING.
		if(anim.GetCurrentAnimatorStateInfo(0).IsName("bat_move"))
		{
			if (controller.isGrounded) {
				if (target != null) {
					agent.SetDestination (target.transform.position);
				}

			}
		}
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("bat_attack")) {
			if (target == null) { //attacking a no-longer existing target.
				food = scanForFood();
				lookForMeal (food);
			}
		}
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("bat_die")) {
			Destroy (this);
		}

	}
	protected override void breed()
	{
		base.breed ();	
	}
	protected override GameObject[] scanForFood()
	{
		GameObject[] foodItems = GameObject.FindGameObjectsWithTag ("Prey");
		if (foodItems.Length == 0) {
			return null;
		} else {
			return foodItems;
		}
	}

	protected override void hurt(bool flag)
	{
		base.hurt (flag);
	}

	protected override void isDead(bool flag)
	{
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("bat_die")) {
			Debug.Log ("nani.");
			Destroy (gameObject);
		}
		Debug.Log ("Sumting broke.");
	}

	protected override void feed(bool flag)
	{
//		Debug.Log ("PREY IS WITHIN SIGHT"); //TESTING.
		base.feed(flag);
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("bat_attack")) 
		{
		//	Debug.Log ("target is : " + currDistance+" away.");

			if (target != null && (currDistance <= attackZone && foundPrey == true))
			{
			//	Debug.Log ("I'MA FIRIN' MA LAZ0R");
				Destroy (target.gameObject);
				foundPrey = false; //prey is eaten, pick a new one.

						fullness += 0.5f;
						if (fullness >= 1) 
						{
							breed ();
						}
			}
		}
	}
}