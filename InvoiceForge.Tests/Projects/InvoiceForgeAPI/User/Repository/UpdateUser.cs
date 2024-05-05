using FunctionalTests.Projects.InvoiceForgeApi;
using InvoiceForgeApi.Data.SeedClasses;

using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Models;
using Xunit;

namespace Repository
{
    [Collection("Sequential")]
    public class UpdateUser: WebApplicationFactory
    {
        [Fact]
        public Task CanUpdateUserbyId()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                
                //ASSERT
                var updateUser = new UserUpdateRequest
                {
                    AuthenticationId = 123456789
                };

                var updateUserResult = await db._repository.User.Update(1, updateUser);

                await db._repository.Save();
                Assert.True(updateUserResult);

                var updatedUser = await db._context.User.FindAsync(1);
                Assert.NotNull(updatedUser);
                Assert.IsType<User>(updatedUser);

                if (updatedUser is not null)
                {
                    Assert.Equal(updatedUser.AuthenticationId, updateUser.AuthenticationId);
                }

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorWhenUpdateNonExistentUser()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                
                
                //ASSERT
                var entity = new UserUpdateRequest{ AuthenticationId=1234567890 };
                try
                {
                    var updateResult = await db._repository.User.Update(100, entity);
                    await db._repository.Save();
                } catch(Exception error) {
                    Assert.IsType<NoEntityError>(error);
                }

                //CLEAN
                db.Dispose();
            });
        } 

        [Fact]
        public Task ThrowErrorWhenUpdateUserWithAuthIdOfAnotherUser()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                
                var users = new UserSeed().Populate();

                //ASSERT
                var entity = new UserUpdateRequest { AuthenticationId = users[0].AuthenticationId};
                
                try
                {
                    var updateResult = await db._repository.User.Update(users[1].Id ,entity);
                    await db._repository.Save();
                }
                catch (Exception error)
                {
                    Assert.IsType<NoEntityError>(error);
                }

                //CLEAN
                db.Dispose();
            });
        }     
    }
}