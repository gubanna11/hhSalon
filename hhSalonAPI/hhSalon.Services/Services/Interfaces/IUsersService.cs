using hhSalon.Domain.Entities;
using hhSalon.Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hhSalon.Services.Services.Interfaces
{
	public interface IUsersService
	{
		Task<IEnumerable<User>> GetAllUser();

		User GetUserById(string id);
		
	}
}
