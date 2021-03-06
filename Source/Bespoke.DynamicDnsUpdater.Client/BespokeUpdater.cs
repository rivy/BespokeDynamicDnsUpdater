﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Bespoke.DynamicDnsUpdater.Client.DnsOMatic;
using Bespoke.DynamicDnsUpdater.Client.Dnsimple;
using Bespoke.DynamicDnsUpdater.Client.Route53;
using Bespoke.DynamicDnsUpdater.Common;
using DNSimple;

namespace Bespoke.DynamicDnsUpdater.Client
{
	public class BespokeUpdater
	{
		public BespokeUpdater(DynamicDnsClientBase dynamicDnsClient)
		{
			Client = dynamicDnsClient;
		}

		public BespokeUpdater(DynamicDnsUpdaterClientType clientType)
			: this(GetClient(clientType))
		{
		}

		public BespokeUpdater(int clientTypeid)
			:this(GetClient(clientTypeid))
		{
		}

		public DynamicDnsClientBase Client { get; set; }

		/// <summary>
		/// Gets the client with for the given DynamicDnsUpdaterClientType
		/// and initializes with the values in the Config file.  Default is DnsOMaticClient.
		/// </summary>
		/// <param name="clientType">The type of the dynamic dns client.</param>
		/// <returns>The dynamic dns Client.</returns>
		public static DynamicDnsClientBase GetClient(DynamicDnsUpdaterClientType clientType)
		{
			switch(clientType)
			{
				case DynamicDnsUpdaterClientType.Route53:
					return new Route53Client();
				case DynamicDnsUpdaterClientType.Dnsimple:
					if(!string.IsNullOrWhiteSpace(Config.DnsimpleApiToken))
						return new DnsimpleClient(Config.DnsimpleUsername, new ApiToken(Config.DnsimpleApiToken));
					else
						return new DnsimpleClient();
				default: 
					return new DnsOMaticClient();
			}
		}

		public static DynamicDnsClientBase GetClient(int clientTypeId)
		{
			var type = (DynamicDnsUpdaterClientType)clientTypeId;

			var client = GetClient(type);

			return client;
		}
	}
}
