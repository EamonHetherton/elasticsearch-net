﻿using System.Threading.Tasks;
using Elastic.Xunit.XunitPlumbing;
using Nest;
using Tests.Framework.EndpointTests;

namespace Tests.XPack.CrossClusterReplication.Follow.CreateFollowIndex
{
	public class CreateFollowIndexUrlTests : UrlTestsBase
	{
		[U] public override async Task Urls()
		{
			var name = "x";
			await UrlTester.PUT($"/{name}/_ccr/follow")
				.Fluent(c => c.CrossClusterReplication.CreateFollowIndex(name, d => d))
				.Request(c => c.CrossClusterReplication.CreateFollowIndex(new CreateFollowIndexRequest(name)))
				.FluentAsync(c => c.CrossClusterReplication.CreateFollowIndexAsync(name, d => d))
				.RequestAsync(c => c.CrossClusterReplication.CreateFollowIndexAsync(new CreateFollowIndexRequest(name)));

		}
	}
}
