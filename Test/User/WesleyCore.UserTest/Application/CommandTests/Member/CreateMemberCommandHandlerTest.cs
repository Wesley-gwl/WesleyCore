using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using WesleyCore.User.Application.Commands.Member;
using WesleyCore.User.Domain;
using Xunit;

namespace WesleyCore.UserTest.Application.CommandTests.Member
{
    /// <summary>
    /// 创建会员单元测试
    /// </summary>
    public class CreateMemberCommandHandlerTest
    {
        private readonly Mock<IMemberRepository> _memberRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IConfiguration> _configuration;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CreateMemberCommandHandlerTest()
        {
            _memberRepositoryMock = new Mock<IMemberRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _configuration = new Mock<IConfiguration>();
        }

        /// <summary>
        /// 通过测试用例
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Handle_return_true()
        {
            var fakeMemberCommand = new CreateMemberCommand() { Company = "测试成功", Password = "123456", PhoneNumber = "1366" + DateTime.Now.ToString("yyyyMMddhh"), UserName = "测试成功" };
            _memberRepositoryMock.Setup(p => p.UnitOfWork.SaveChangesAsync(default)).Returns(Task.FromResult(1));
            _memberRepositoryMock.Setup(p => p.AnyAsync(p => true)).Returns(Task.FromResult(true));
            var fakeMember = new User.Domain.Member(fakeMemberCommand.UserName, fakeMemberCommand.PhoneNumber, fakeMemberCommand.Company, 5);
            _memberRepositoryMock.Setup(p => p.InsertAndGetIdAsync(It.IsAny<User.Domain.Member>(), default)).Returns(Task.FromResult(fakeMember));

            _userRepositoryMock.Setup(p => p.AnyAsync(p => true)).Returns(Task.FromResult(true));

            _configuration.Setup(p => p["Allocation:allowUserNumber"]).Returns("5");
            _memberRepositoryMock.Setup(p => p.UnitOfWork.SaveEntitiesAsync(default)).Returns(Task.FromResult(true));
            //Act
            var handler = new CreateMemberCommandHandler(_memberRepositoryMock.Object, _userRepositoryMock.Object, _configuration.Object);
            var result = await handler.Handle(fakeMemberCommand, new CancellationToken());

            //Assert
            Assert.True(result);
        }
    }
}