using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LofovaChyse.Class
{
	public class Cache
	{
		private static Dictionary<string, CacheSession> Sessions = new Dictionary<string, CacheSession>();
		private static HttpSessionStateBase SesRequest = null;

		private static string GetSessionID(HttpSessionStateBase Session)
		{
			return Session != null ? Session.SessionID : "";
		}

		public static void SessionRequest(HttpSessionStateBase Session)
		{
			SesRequest = Session;
		}

		public static CacheSession Open(HttpSessionStateBase Session)
		{
			SesRequest = null;
			if (Sessions.TryGetValue(GetSessionID(Session), out CacheSession cacheSession))
			{
				return cacheSession;
			}

			cacheSession = new CacheSession
			{
				Session = Session
			};

			Sessions.Add(GetSessionID(Session), cacheSession);

			return cacheSession;
		}

		public static CacheSession Open()
		{
			if (SesRequest == null)
				return null;

			return Open(SesRequest);
		}

		private static void Flush(string SessionID)
		{
			Sessions.Remove(SessionID != null ? SessionID : "");
		}

		public class CacheSession
		{
			public HttpSessionStateBase Session;
			private Dictionary<string, Object> Data = new Dictionary<string, object>();

			public CacheSession Set(string key, Object value)
			{
				if (Data.ContainsKey(key))
					Data.Remove(key);
				Data.Add(key, value);

				return this;
			}

			public T Get<T>(string key)
			{
				if (!Data.TryGetValue(key, out object value))
					return default(T);
				return (T)value;
			}

			public void Flush()
			{
				Cache.Flush(Session?.SessionID);
			}
		}
	}

	
}