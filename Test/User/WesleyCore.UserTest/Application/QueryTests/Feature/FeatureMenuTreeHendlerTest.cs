using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WesleyCore.User.Application;
using WesleyCore.User.Domain;
using WesleyCore.User.Domain.Repository;
using Xunit;

namespace WesleyCore.UserTest.Application.QueryTests.Feature
{
    /// <summary>
    /// 菜单树单元测试
    /// </summary>
    public class FeatureMenuTreeHendlerTest
    {
        private readonly Mock<IFeatureRepository> _featureRepositoryMock;

        public FeatureMenuTreeHendlerTest()
        {
            _featureRepositoryMock = new Mock<IFeatureRepository>();
        }

        /// <summary>
        /// 超级管理员获取菜单
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Handle_user_admin_return_menu()
        {
            var fakeCommand = new GetFeatureMenuTreeInput() { IsAdmin = true };
            var fakeMenu = GetFakeMenu();
            _featureRepositoryMock.Setup(p => p.GetAll()).Returns(fakeMenu.AsQueryable());
            //_featureRepositoryMock.Setup(p => p.GetAll().Where(It.IsAny<Expression<Func<User.Domain.Feature, bool>>>())).Returns(fakeMenu.AsQueryable());
            //Act
            var handler = new FeatureMenuTreeHendler(_featureRepositoryMock.Object);
            var result = await handler.Handle(fakeCommand, new CancellationToken());

            //Assert
            Assert.True(result.Count > 0);
        }

        /// <summary>
        /// 员工获取菜单
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Handle_user_not_admin_return_menu()
        {
            var fakeCommand = new GetFeatureMenuTreeInput() { IsAdmin = false };
            var fakeMenu = GetFakeMenu();
            _featureRepositoryMock.Setup(p => p.GetUserPCMenuFeatureList(new Guid())).Returns(Task.FromResult(fakeMenu));

            //Act
            var handler = new FeatureMenuTreeHendler(_featureRepositoryMock.Object);
            var result = await handler.Handle(fakeCommand, new CancellationToken());

            //Assert
            Assert.True(result.Count > 0);
        }

        /// <summary>
        /// 获取菜单构造数据
        /// </summary>
        /// <returns></returns>
        private static List<User.Domain.Feature> GetFakeMenu()
        {
            return new List<User.Domain.Feature>
            {
                new User.Domain.Feature(null, "测试", FeatureTypeEnum.PC菜单, "", "", true, false, "", null, ""),
                new User.Domain.Feature(null, "测试2", FeatureTypeEnum.PC菜单, "", "", true, false, "", null, ""),
                new User.Domain.Feature(null, "测试3", FeatureTypeEnum.PC菜单, "", "", true, false, "", null, ""),
            };
        }
    }
}