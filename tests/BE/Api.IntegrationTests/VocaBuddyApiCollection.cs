namespace Api.IntegrationTests;

[CollectionDefinition("VocaBuddy API collection", DisableParallelization = false)]
public class VocaBuddyApiCollection : ICollectionFixture<VocaBuddyApiFactory>;