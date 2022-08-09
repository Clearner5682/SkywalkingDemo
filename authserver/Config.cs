using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
                new IdentityResources.OpenId()
        };

    public static List<TestUser> TestUsers =>
        new List<TestUser>
        {
                new TestUser{ SubjectId="1", Username="admin",Password="123456"},
                new TestUser{ SubjectId="2", Username="hongyan",Password="123456"}
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
                new ApiResource("identity-service","identity-service"){ Scopes=new List<string>{ "identity-service"} },// 要注意，这里的Scopes必须和ApiScopes中的一致（也和验证的Audience一致）
                new ApiResource("base-setting-service","base-setting-service"){ Scopes=new List<string>{ "base-setting-service"}},
                new ApiResource("process-control-service","process-control-service"){ Scopes=new List<string>{ "process-control-service"}}
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("identity-service","identity-service"),
            new ApiScope("base-setting-service","base-setting-service"),
            new ApiScope("process-control-service","process-control-service")
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
                new Client()
                {
                    ClientId="aio",
                    ClientName="aio",
                    ClientSecrets=new List<Secret>{ new Secret("aio".Sha256())},
                    AllowedGrantTypes=new string[]{ GrantType.ClientCredentials,GrantType.ResourceOwnerPassword },
                    AllowedScopes=new List<string>{ "identity-service", "base-setting-service","process-control-service" },
                }
        };
}