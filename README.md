# WesleyCore
介绍
此微服务系统使用DDD开发模式,.net5.0+sqlserver.采用租户模式设计。
其中运用到 
ocelot 网关负载 
consul 服务注册发现-健康检测
Identity4 鉴权授权
中介者MediatR代理
CQRS读写分离模式 
CAP框架+Rabbitmq消息队列
grpc远程调用 
reids(可配置集群-主从复制)  
仓储 
聚合 
工作单元
垂直分库


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

UserAggregation http://localhost:6001
用户聚合服务 使用grpc调用多个微服务进行数据整合

IdentityServer http://localhost:3000
认证授权中心，登入授权功能

WesleyCore.User https://localhost:5003
用户微服务 包含会员聚合 用户聚合 角色聚合 菜单权限聚合

WesleyCore.Customer https://localhost:5004
客户微服务 包含客户分类聚合 客户聚合

WesleyCore.Message https://localhost:5005
消息微服务
