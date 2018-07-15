﻿using System;
using UnityEngine.Networking;

namespace Momiji
{
	public abstract class GetRequestable<Param, Res> : Requestable<Param, Res> where Param : IParameterizable where Res : IResponsible
	{
		public IObservable<Res> Get ()
		{
			Uri uri = new Uri (HostName + Path);
			data = UnityWebRequest.Get (uri);
			Header?.ForEach (_ =>
			{
				data.SetRequestHeader (_.Key, _.Value);
			});
			return this.ResponseData ();
		}
	}
}