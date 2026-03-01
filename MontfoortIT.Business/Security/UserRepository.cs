using System;
using System.Collections.Generic;
using System.Text;
using MontfoortIT.Business.DataInterfaces.Security;

namespace MontfoortIT.Business.Security
{
    public class UserRepository : RepositoryBase<User<BusinessUser, IUserDto>, DataInterfaces.Security.IUserTable<DataInterfaces.Security.IUserDto>, DataInterfaces.Security.IUserDto>
    {
        public UserRepository(ISecurityConnection securityConnection, IUserTable<IUserDto> table, IFactory< User<BusinessUser,IUserDto>, IUserDto> factory) 
            : base(securityConnection, table, factory)
        {
        }
    }
}
