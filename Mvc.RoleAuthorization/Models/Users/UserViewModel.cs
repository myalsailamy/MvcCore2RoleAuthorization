namespace Mvc.RoleAuthorization.Models
{
	public class UserViewModel
	{
		public int Id { get; set; }

		public string UserName { get; set; }

		public string Email { get; set; }

		public RoleViewModel[] Roles { get; set; }
	}
}