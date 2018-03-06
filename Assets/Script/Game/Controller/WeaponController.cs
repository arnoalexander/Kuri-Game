using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class WeaponController : Element
	{
		 public void CreateBall(){
			var mousepos = Input.mousePosition;
			var pointpos = Camera.main.ScreenToWorldPoint (mousepos);
			pointpos.y = 5;
			pointpos.z = 5;
			Instantiate (app.model.weaponModel.ball, pointpos, transform.rotation);
		}
	}
}

