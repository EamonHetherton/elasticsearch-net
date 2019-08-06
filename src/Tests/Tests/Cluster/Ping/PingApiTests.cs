﻿using Elasticsearch.Net;
using Nest;
using Tests.Core.ManagedElasticsearch.Clusters;
using Tests.Framework.EndpointTests;
using Tests.Framework.EndpointTests.TestState;

namespace Tests.Cluster.Ping
{
	public class PingApiTests : ApiIntegrationTestBase<ReadOnlyCluster, PingResponse, IPingRequest, PingDescriptor, PingRequest>
	{
		public PingApiTests(ReadOnlyCluster cluster, EndpointUsage usage) : base(cluster, usage) { }

		protected override bool ExpectIsValid => true;
		protected override int ExpectStatusCode => 200;
		protected override HttpMethod HttpMethod => HttpMethod.HEAD;
		protected override string UrlPath => "/";

		protected override LazyResponses ClientUsage() => Calls(
			(client, f) => client.Ping(),
			(client, f) => client.PingAsync(),
			(client, r) => client.Ping(r),
			(client, r) => client.PingAsync(r)
		);
	}
}
