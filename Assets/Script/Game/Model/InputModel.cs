using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class InputModel : Element {

		// threshold swipe touch
		public float swipeDeltaThreshold = 125;

		// status touch
		public bool tap { get; set;}
		public bool tapPrev { get; set;}
		public bool swipeLeft { get; set;}
		public bool swipeLeftPrev { get; set;}
		public bool swipeRight { get; set;}
		public bool swipeRightPrev { get; set;}
		public bool swipeUp { get; set;}
		public bool swipeUpPrev { get; set;}
		public bool swipeDown { get; set;}
		public bool swipeDownPrev { get; set;}
		public bool isDragging { get; set;}
		public Vector2 startTouch { get; set;}
		public Vector2 swipeDelta { get; set;}

	}
}

