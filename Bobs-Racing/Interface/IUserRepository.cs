using Bobs_Racing.Models;

namespace Bobs_Racing.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task AddUserAsync(User user);
        /*
        Task UpdateUserCredentialsAsync(User user);
        Task UpdateUserCreditsAsync(User user); */

        Task UpdateUserAsync(int userId, UserDTO userDto);

        Task<UserWithBetsDTO> GetUserWithBetsAsync(int userId);


        Task DeleteUserAsync(int id);
        Task<User> GetUserByUsernameAsync(string username);

        /*// Check if a username is already taken
        Task<bool> IsUsernameTakenAsync(string username);*/
    }
}
