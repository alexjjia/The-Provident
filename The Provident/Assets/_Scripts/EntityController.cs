using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

[System.Serializable]
public class EntityController : MonoBehaviour {
	protected float updateRate; // 0.5f to 10, keeps track of how often an action is performed.
	protected float fullness; // 0 to 1, when 1, calls breed()
	protected float behaviorRate; // 0 to 100, used to determine decision tree.
	protected float behaviorPicker; // rng used in Update()

	protected float timer; //time tracker used in Update()
	protected float hungerTimer;
	protected Animator anim;
	protected CharacterController controller;
	protected NavMeshAgent agent;

	protected bool foundPrey; //flag for whether or not a target has been found.
	protected GameObject[] food; //keeps track of all available food items.
	protected float closestPrey; //initial distance to closest target.
	protected GameObject target; 
	protected float currDistance; //current distance to target.
	protected float attackZone;
	protected float hungerRate;


	// Use this for initialization
	protected virtual void Start () {
		foundPrey = false;
		attackZone = 2.3f;
		agent = GetComponent<NavMeshAgent> ();
		controller = GetComponent<CharacterController> ();
		anim = GetComponent<Animator> ();
		timer = 0;
		hungerRate = 5f;
		updateRate = Random.Range (1, 5);
		fullness = Random.Range (0.3f, 0.6f);
		behaviorRate = Random.Range(20,80);
		food = scanForFood ();
	}
	
	// Update is called once per frame
	protected virtual void Update () 
	{
//		Debug.Log ("Look for food: " + foundPrey);
		if (updateRate < 0) 
		{
			updateRate = 0.5f;
		}

		timer += Time.deltaTime;
		hungerTimer += Time.deltaTime;
		//Debug.Log ("Time is at: " + hungerTimer);
		if (hungerTimer > hungerRate) 
		{
			
			Debug.Log ("Hunger is now at: " + fullness);
			if (fullness < 0) {
				Debug.Log ("Yer dead, bro.");
				idle (false);
				move (false);
				hurt (false);
				isDead (true);
			} else {
				fullness -= 40;
				idle (false);
				move (false);
				isDead (false);
				hurt (true);
			}
			hungerTimer = 0;
		}

		if (timer <= updateRate) 
		{
			idle (true);
			move (false);
			feed (false);
		} 
		else //timer > updateRate
		{
			behaviorPicker = Random.Range (0, 100);
			idle (false);

			if (behaviorPicker <= behaviorRate)  //feed()
			{
				//if no more food is to be had, rescan for food.
				if (food == null) 
				{
					food = scanForFood ();
					timer = 0; //reset the timer and try again.
				}
				if (foundPrey == false) {
					lookForMeal (food);
				}
				//if still no food to be found, do nothing.

				else //food exists, now we just gotta find it! 
				{
					if (target == null) 
					{
						feed (false);
						move (false);
						idle (true);
					}
					else 
					{ //implies that target != null, && foundPrey == true.
						currDistance = Vector3.Distance (target.transform.position, transform.position);
						//if prey is within attack range of predator, eat.
						if (currDistance <= attackZone) {  
						//	Debug.Log ("Om nom time");
							feed (true);
							move (false);
						} else {
							move (true);
							feed (false);
						}
					}		
				}
			} 
			else //move()
			{
				moveElsewhere();
			}
		//	timer = 0; //resets timer.
		}

	}

	protected virtual void FixedUpdate()
	{

	}

	//To be overridden in specfic entity classes.
	protected virtual GameObject[] scanForFood()
	{
		return GameObject.FindGameObjectsWithTag ("");
		//		lookForMeal (food);
	}

	protected virtual void lookForMeal(GameObject[] prey)
	{
		if (prey == null) {
			return;
		}
		target = null;
		float distance = Mathf.Infinity;
		foreach (GameObject food in prey) 
		{
			if (food == null) {
				continue;
			}
			closestPrey = Vector3.Distance (food.transform.position, this.transform.position);
			if (closestPrey < distance) 
			{
				target = food;
				distance = closestPrey;
			}
		}
		//if we actually found a target, set flag to true.
		if (target != null) {
			foundPrey = true;
		}
		
	}

	protected virtual void breed()
	{
		
		fullness = Random.Range(0.1f, 0.3f); //resets hunger to prevent infinite spawning.
		updateRate += Random.Range (-2f, 3f); //changes stats for children.
		behaviorRate += Random.Range (-5f, 5f);
		moveElsewhere ();
		Instantiate(this); //clones self.
		moveElsewhere();

		updateRate -= Random.Range (0, 2f); //gets weaker as a result.
		behaviorRate -= Random.Range (0, 2f);
	}

	protected virtual void feed(bool flag)
	{
		anim.SetBool("isAttacking", flag);
//		fullness += 0.05f;
//		if (fullness >= 1) 
//		{
//			breed ();
//		}
	}

	protected virtual void hurt(bool flag)
	{
		anim.SetBool ("isHit", flag);
	}

	protected virtual void isDead(bool flag)
	{
		anim.SetBool ("isDead", flag);
	}

	protected void idle(bool flag)
	{
		anim.SetBool ("isIdle", flag);
	}

	protected void move(bool flag)
	{	

		anim.SetBool ("isMoving", flag);
	}

	protected void moveElsewhere()
	{
		agent.SetDestination (this.transform.position + Vector3.one * Random.Range (-5f, 5f));
	}
}
