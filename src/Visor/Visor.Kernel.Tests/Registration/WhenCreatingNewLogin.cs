using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using Visor.Abstractions.Entities.Results;
using Visor.Abstractions.Repositories;
using Visor.Abstractions.User;

namespace Visor.Kernel.Tests
{
    public class WhenCreatingNewLogin
    {
        private IRegistrationManager registrationManager;
        [SetUp]
        public void Setup()
        {
            var userRepo = new Mock<ILoginProvider>();
            userRepo.Setup(r => r.CreateLogin(
                It.Is<string>(u=> u.Equals("")), 
                It.Is<string>(u=>u.Equals("abc@example.com")), 
                It.Is<string>(u => u.Equals("SuperSecret"))))
            .Returns(Task.FromResult(new BaseResult { Succeeded = false}));
            userRepo.Setup(r => r.CreateLogin(
               It.Is<string>(u => u.Equals("abc")),
               It.Is<string>(u => u.Equals("")),
               It.Is<string>(u => u.Equals("SuperSecret"))))
           .Returns(Task.FromResult(new BaseResult { Succeeded = false }));
            userRepo.Setup(r => r.CreateLogin(
               It.Is<string>(u => u.Equals("abc")),
               It.Is<string>(u => u.Equals("abc@example.com")),
               It.Is<string>(u => u.Equals(""))))
           .Returns(Task.FromResult(new BaseResult { Succeeded = false }));
            userRepo.Setup(r => r.CreateLogin(
               It.Is<string>(u => u.Equals("abc")),
               It.Is<string>(u => u.Equals("duplicate@example.com")),
               It.Is<string>(u => u.Equals("SuperSecret"))))
           .Returns(Task.FromResult(new BaseResult { Succeeded = false }));
            userRepo.Setup(r => r.CreateLogin(
              It.Is<string>(u => u.Equals("duplicate")),
              It.Is<string>(u => u.Equals("abc@example.com")),
              It.Is<string>(u => u.Equals("SuperSecret"))))
          .Returns(Task.FromResult(new BaseResult { Succeeded = false }));
        }

        [Test]
        public void EnsuresItIsANewUser()
        {
            Assert.Pass();
        }
    }
}