# WesleyCore
此项目启动先置条件
consul
redis
rabbitmq
sqlserver最好2014以上版本
vs2019最新版本
分享插件地址https://share.weiyun.com/2lNrVEys

consul 默认8500端口
redis 默认6379端口
rabbitmq 默认5672端口

运行启动
再consul目录下运行 consul.exe agent -dev
consul 负责服务注册发现，负载均衡等功能
确保rabbitmq服务运行和redis服务运行


WesleyPC.Gateway https://localhost:5000;
为PC端网关服务，统一前端入口 后续可以添加微信小程序 app等不同网关服务

IdentityServer http://localhost:3000
认证授权中心，登入授权功能

WesleyCore.User https://localhost:5003
用户微服务 包含会员聚合 用户聚合 角色聚合 菜单权限聚合

WesleyCore.Customer https://localhost:5004
客户微服务 包含客户分类聚合 客户聚合

UserAggregation http://localhost:6001
用户聚合服务 使用grpc调用多个微服务进行数据整合
