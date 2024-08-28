using hhSalon.Domain.Entities;

namespace hhSalon.Services.Services.Implementations
{
    public class ChatService
    {
        private static readonly Dictionary<User, string> Users = new Dictionary<User, string>();

        public bool AddUserToList(User userToAdd)
        {
            lock (Users)
            {
                foreach (var user in Users)
                {
                    if (user.Key.Id == userToAdd.Id)
                    {
                        return false;
                    }
                }

                Users.Add(userToAdd, null);
                return true;
            }
        }

        public void AddUserConnectionId(User user, string connectionId)
        {
            lock (Users)
            {
                if (Users.Where(u => u.Key.Id == user.Id).Count() != 0)
                {
                    var u = Users.Where(u => u.Key.Id == user.Id).FirstOrDefault();
                    Users[u.Key] = connectionId;
                }
            }
        }

        public User GetUserByConnectionId(string connectionId)
        {
            lock (Users)
            {
                return Users.Where(x => x.Value == connectionId).Select(x => x.Key).FirstOrDefault();
            }
        }

        public string GetConnectionIdByUser(User user)
        {
            lock (Users)
            {
                return Users.Where(x => x.Key.Id == user.Id).Select(x => x.Value).FirstOrDefault();
            }
        }


        public void RemoveUserFromList(User user)
        {
            lock (Users)
            {
                if (user != null)
                {
                    Users.Remove(Users.Where(x => x.Key.Id == user.Id).Select(x => x.Key).FirstOrDefault());
                }
            }
        }

    }
}
