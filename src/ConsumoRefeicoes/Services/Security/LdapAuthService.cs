using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Threading.Tasks;
using backend.Interfaces.Services;
using backend.Interfaces.Services.Security;
using backend.Models;
using Microsoft.Extensions.Options;

namespace backend.Services.Security
{
    public class LdapAuthService : IAuthentication
    {

        private readonly LdapConfig _config;
        protected readonly DirectoryEntry _ldapConnection;
        protected User _user;

        protected readonly IUserGeral _userGeralService;

        public LdapAuthService(IOptions<LdapConfig> ldapConfig, IUserGeral userGeralService)
        {
            _config = ldapConfig.Value;
            _userGeralService = userGeralService;
        }

        public async Task<User> Autenthicate(string username, string password)
        {

            DirectoryEntry ldapConnection = ConnectLdap(username, password);

            object isValidPassword = null;
            try
            {
                // authenticate (check password)
                isValidPassword = ldapConnection.NativeObject;
            }
            catch (Exception ex)
            {
                // _logger.Log.Debug($"LDAP Authentication Failed for {domainAndUsername}"); 
                throw new ApplicationException(ex.Message);
            }


            DebugProperties(username);
            List<Group> listGroups = GetAllGroups(username);
            bool groupsVerify = VerirfyGroup(listGroups);
            if (!groupsVerify)
            {
                throw new ApplicationException("O usuário não tem permissão para acessar este sistema!!!");
            }

            string nameOfUser = GetUserFullName(username);
            var _userGeral = _userGeralService.GetUsers(username);
            var matricula = _userGeral.Result.Where(index => index.NumCadastro != 0).Last();

            _user = new User
            {
                Matricula = matricula.NumCadastro,
                Username = username,
                Name = nameOfUser,
                Groups = listGroups
            };


            return _user;
        }

        private DirectoryEntry ConnectLdap(string username, string password)
        {
            string bindDN = "LDAP://" + _config.BindDn;


            DirectoryEntry ldapConnection = new DirectoryEntry(_config.Server);
            ldapConnection.Path = bindDN;
            ldapConnection.Username = username;
            ldapConnection.Password = password;

            ldapConnection.AuthenticationType = AuthenticationTypes.Secure;

            return ldapConnection;
        }

        private List<Group> GetAllGroups(string username)
        {
            List<Group> listGroups = new List<Group>();

            ResultPropertyCollection fields = GetAllProperties(username);

            foreach (String ldapField in fields.PropertyNames)
            {
                // cycle through objects in each field e.g. group membership  
                // (for many fields there will only be one object such as name)  
                foreach (Object myCollection in fields[ldapField])
                    if (ldapField == "memberof")
                    {
                        string groupNameTmp = myCollection.ToString();
                        string[] groupNameParts = groupNameTmp.Split(',');
                        string[] groupNameParts2 = groupNameParts[0].Split('=');
                        string _groupName = groupNameParts2[1];


                        Group group = new Group
                        {
                            GroupName = _groupName
                        };

                        listGroups.Add(group);
                    }
            }

            return listGroups;

        }

        private bool VerirfyGroup(List<Group> groups)
        {
            List<Group> listGroups = new List<Group>();

            foreach (var ldapField in groups)
            {
                if (ldapField.GroupName == "G_NUTRICAO" || ldapField.GroupName == "G_TI")
                {
                    return true;
                }
                // cycle through objects in each field e.g. group membership  
                // (for many fields there will only be one object such as name)                  
            }
            return false;

        }



        private ResultPropertyCollection GetAllProperties(string username)
        {
            DirectorySearcher search = new DirectorySearcher(_ldapConnection);

            search.Filter = String.Format(_config.SearchFilter, username);
            SearchResult result = search.FindOne();

            ResultPropertyCollection fields = result.Properties;

            return fields;
        }

        private string GetUserFullName(string username)
        {
            ResultPropertyCollection fields = GetAllProperties(username);
            string name = "";

            foreach (String ldapField in fields.PropertyNames)
            {
                // cycle through objects in each field e.g. group membership  
                // (for many fields there will only be one object such as name)  
                foreach (Object myCollection in fields[ldapField])
                    if (ldapField == "name")
                    {
                        name = myCollection.ToString();
                    }

            }
            return name;
        }

        private void DebugProperties(string username)
        {
            ResultPropertyCollection fields = GetAllProperties(username);
            // DirectorySearcher search = new DirectorySearcher(_ldapConnection);

            // search.Filter = String.Format(_config.SearchFilter, username);
            // SearchResult result = search.FindOne();

            // ResultPropertyCollection fields = result.Properties;

            foreach (String ldapField in fields.PropertyNames)
            {
                // cycle through objects in each field e.g. group membership  
                // (for many fields there will only be one object such as name)  
                foreach (Object myCollection in fields[ldapField])
                    Console.WriteLine(String.Format("{0,-20} : {1}",
                                  ldapField, myCollection.ToString()));
            }
        }

        public bool BelongToGroup(string groupName)
        {

            Group group = _user.Groups.Where(u => u.GroupName.Contains(groupName)).FirstOrDefault();
            return group is Group;
        }
    }
}