﻿using System;
using Elastic.Xunit.XunitPlumbing;
using Elasticsearch.Net;
using FluentAssertions;
using Nest;
using Tests.Core.ManagedElasticsearch.Clusters;
using Tests.Framework.EndpointTests;
using Tests.Framework.EndpointTests.TestState;
using static Elastic.Managed.Ephemeral.ClusterAuthentication;

namespace Tests.XPack.Security.User.GetUserAccessToken
{
	[SkipVersion("<5.5.0", "")]
	public class GetUserAccessTokenApiTests
		: ApiIntegrationTestBase<XPackCluster, GetUserAccessTokenResponse, IGetUserAccessTokenRequest,
			GetUserAccessTokenDescriptor, GetUserAccessTokenRequest>
	{
		public GetUserAccessTokenApiTests(XPackCluster cluster, EndpointUsage usage) : base(cluster, usage) { }

		protected override bool ExpectIsValid => true;

		protected override object ExpectJson => new
		{
			grant_type = "password",
			scope = "FULL",
			username = Admin.Username,
			password = UserPassword
		};

		protected override int ExpectStatusCode => 200;

		protected override Func<GetUserAccessTokenDescriptor, IGetUserAccessTokenRequest> Fluent => d => d
			.Scope("FULL");

		protected override HttpMethod HttpMethod => HttpMethod.POST;

		protected override GetUserAccessTokenRequest Initializer => new GetUserAccessTokenRequest(Admin.Username, UserPassword)
		{
			Scope = "FULL"
		};

		protected override bool SupportsDeserialization => false;

		protected override string UrlPath => "/_security/oauth2/token";

		protected virtual string UserPassword => Admin.Password;

		protected override LazyResponses ClientUsage() => Calls(
			(client, f) => client.Security.GetUserAccessToken(Admin.Username, UserPassword, f),
			(client, f) => client.Security.GetUserAccessTokenAsync(Admin.Username, UserPassword, f),
			(client, r) => client.Security.GetUserAccessToken(r),
			(client, r) => client.Security.GetUserAccessTokenAsync(r)
		);

		protected override GetUserAccessTokenDescriptor NewDescriptor() => new GetUserAccessTokenDescriptor(Admin.Username, UserPassword);

		protected override void ExpectResponse(GetUserAccessTokenResponse response)
		{
			response.AccessToken.Should().NotBeNullOrEmpty();
			response.Type.Should().NotBeNullOrEmpty().And.Be("Bearer");
			response.ExpiresIn.Should().BeGreaterThan(0);
			response.Scope.Should().Be("full");
		}
	}

	public class GetUserAccessTokenBadPasswordApiTests : GetUserAccessTokenApiTests
	{
		public GetUserAccessTokenBadPasswordApiTests(XPackCluster cluster, EndpointUsage usage) : base(cluster, usage) { }

		protected override bool ExpectIsValid => false;
		protected override int ExpectStatusCode => 401;
		protected override string UserPassword => "bad_password";

		protected override void ExpectResponse(GetUserAccessTokenResponse response) => response.ServerError.Should().NotBeNull();
	}
}
