﻿using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Momiji
{
	public class SampleMockRequest : MockRequestable, IMockSendRequest, ISampleRequest
	{
		public Requestable Request => this;
		public void Get () => this.Send ();
	}
}