using hhSalon.Domain.Entities.Static;
using hhSalon.Domain.Entities;
using hhSalon.Services.Services.Interfaces;
using hhSalon.Services.ViewModels;
using hhSalonAPI.Domain.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hhSalon.Services.Services.Implementations
{
	public class UsersService: IUsersService
	{
		private readonly AppDbContext _context;
		public UsersService(AppDbContext context)
		{
			_context = context;
		}


		public async Task<IEnumerable<User>> GetAllUser()
		{
			return await _context.Users.ToListAsync();
		}

		public User GetUserById(string id)
		{
			return _context.Users.Where(u => u.Id == id).FirstOrDefault();	
		}


		public async Task UpdateUser(User userObj)
		{
			var user = _context.Users.Where(u => u.Id == userObj.Id).FirstOrDefault();
			if (user != null)
			{
				user.FirstName = userObj.FirstName;
				user.LastName = userObj.LastName;
				user.Email = userObj.Email;
			}

			await _context.SaveChangesAsync();
		}
	}
}
