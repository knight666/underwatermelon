using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.Audio;

namespace Assets.Scripts.Audio 
{
	[Serializable] 
	public class RoundRobinClip 
	{
		public RoundRobinComponent roundRobinComponent = null;
		public float minRand;
		public float maxRand;
		public float minDry = -500;
		public float maxDry = 0;
		public bool inUse = false;
	}
}