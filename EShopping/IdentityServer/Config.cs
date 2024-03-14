using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using ApiScope = IdentityServer4.Models.ApiScope;
using Client = IdentityServer4.Models.Client;
using IdentityResource = IdentityServer4.Models.IdentityResource;
using Secret = IdentityServer4.Models.Secret;
using ApiResource = IdentityServer4.Models.ApiResource;
using TestUser=IdentityServer4.Test.TestUser;
using System.Security.Claims;
using IdentityModel;


namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };


        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
        {
                new ApiScope("api.read"),
                 new ApiScope("api.write"),
        };


        public static List<TestUser> TestUsers =>
           new List<TestUser>
       {
                new TestUser
                {
                    SubjectId="123",
                    Username="vikash",
                    Password="vikash@123",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name,"Vikash Verma")
                    }
                }
       };
        public static IEnumerable<ApiResource> ApiResources =>
           new ApiResource[]
       {
           new ApiResource("inventory")
            {
               Scopes=new List<string> {"api.read","api.write"} ,
                ApiSecrets=new List<Secret>{ new Secret("supersecret".Sha256())} 
           }
        };

        public static IEnumerable<Client> Clients =>
            new Client[]{
                new Client
                {
                    ClientId="client",
                    ClientSecrets={new Secret("secret".Sha256())},
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    AllowedScopes={ "api.write" }
                    
                }
        };

    }
}
