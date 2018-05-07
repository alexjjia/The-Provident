using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PreyController : EntityController {

	public float turnTime;

	protected override void Start()
	{
		base.Start ();
		food = scanForFood();
		turnTime = 2f;


		if(turnTime < 0)
		{
			turnTime = 0;
		}
	}

	protected override void Update () 
	{
		base.Update ();
	//	Debug.Log ("Curr distance to target:" + currDistance);
		if (timer > updateRate && foundPrey == false) 
		{
//			//if even number picked, turn.
			if (behaviorPicker % 2 == 0) {
				Invoke("turn", turnTime);
			}
			timer = 0;
		}
	}

	protected override void FixedUpdate()
	{
//		if(Input.GetKey(KeyCode.Space)) //USED FOR TESTING.
		if(anim.GetCurrentAnimatorStateInfo(0).IsName("rabbit_move"))
		{
			if (controller.isGrounded) {
				
				if (target != null) {
					agent.SetDestination (target.transform.position);
				}

			}
		}

	}

	protected override GameObject[] scanForFood()
	{
		GameObject[] foodItems = GameObject.FindGameObjectsWithTag ("flower");
		if (foodItems.Length == 0) {
			return null;
		} else {
			return foodItems;
		}
	}

	protected override void feed(bool flag)
	{
		//		Debug.Log ("PREY IS WITHIN SIGHT"); //TESTING.
		base.feed(flag);
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("rabbit_attack")) 
		{
			if (target != null && (currDistance <= attackZone && foundPrey == true))
			{
				Destroy (target.gameObject);
				foundPrey = false; //prey is eaten, pick a new one.

				fullness += 1.0f;
				if (fullness >= 1) 
				{
					breed ();
				}
			}
		}
	}

	protected override void hurt(bool flag)
	{
		base.hurt (flag);
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("rabbit_die")) {
			Destroy (this.gameObject);
		}
	}

	protected override void breed()
	{
		turnTime += Random.Range (-3f, 3f);
		base.breed ();
	}

	void turn()
	{
		transform.RotateAround (transform.position, transform.up, Random.Range (-90f, 90f));
	}
		

		
}
