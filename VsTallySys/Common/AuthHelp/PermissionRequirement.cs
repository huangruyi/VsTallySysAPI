﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VsTallySys.Common
{ 
    /// <summary>
    /// 必要参数类，类似一个订单信息
    /// 继承 IAuthorizationRequirement，用于设计自定义权限处理器PermissionHandler
    /// 因为AuthorizationHandler 中的泛型参数 TRequirement 必须继承 IAuthorizationRequirement
    /// </summary>
    public class PermissionRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// 用户权限集合，一个订单包含了很多详情，
        /// 同理，一个网站的认证发行中，也有很多权限详情(这里是Role和URL的关系)
        /// </summary>
        public List<PermissionItem> Permissions { get; set; }
        /// <summary>
        /// 无权限action
        /// </summary>
        public string DeniedAction { get; set; }

        /// <summary>
        /// 认证授权类型
        /// </summary>
        public string ClaimType { internal get; set; }
        /// <summary>
        /// 请求路径
        /// </summary>
        public string LoginPath { get; set; } = "/Api/Login";
        /// <summary>
        /// 发行人
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 订阅人
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public TimeSpan Expiration { get; set; }
        /// <summary>
        /// 刷新时间
        /// </summary>
        public TimeSpan Refresh { get; set; }
        /// <summary>
        /// 签名验证
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }
        /// <summary>
        /// 当前登录用户的角色
        /// </summary>
        //public string CurrUserRole { get; set; }
        /// <summary>
        /// 当前登录用户名
        /// </summary>
        public string CurrUserAccount { get; set; }
        /// <summary>
        /// 当前登录用户ip
        /// </summary>
        public string CurrUserIp { get; set; }


        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="deniedAction">拒约请求的url</param>
        /// <param name="permissions">权限集合</param>
        /// <param name="claimType">声明类型</param>
        /// <param name="issuer">发行人</param>
        /// <param name="audience">订阅人</param>
        /// <param name="signingCredentials">签名验证实体</param>
        /// <param name="expiration">过期时间</param>
        public PermissionRequirement(string deniedAction, List<PermissionItem> permissions, string claimType, string issuer, string audience, SigningCredentials signingCredentials, TimeSpan expiration, TimeSpan refresh)
        {
            ClaimType = claimType;
            DeniedAction = deniedAction;
            Permissions = permissions;
            Issuer = issuer;
            Audience = audience;
            Expiration = expiration;
            Refresh = refresh;
            SigningCredentials = signingCredentials;
        }

        public PermissionRequirement()
        {
        }
    }
}
