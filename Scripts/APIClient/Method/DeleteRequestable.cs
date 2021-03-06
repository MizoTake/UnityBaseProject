﻿using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Momiji
{
	public abstract class DeleteRequestable<Param, Res> : Requestable<Param, Res> where Param : IParameterizable where Res : IResponsible
	{
		protected override UnityWebRequest UpdateRequest (Param param)
		{
			Uri uri = new Uri (HostName + Path);
			if (param is IPathParameterizable)
			{
				uri = new Uri (uri, ((IPathParameterizable) param).QueryPath ());
			}
			var data = UnityWebRequest.Delete (uri);
			Header?.ForEach (x =>
			{
				data.SetRequestHeader (x.Key, x.Value);
			});
			return data;
		}

		public IObservable<Res> Delete => this.ResponseData ();
	}
}