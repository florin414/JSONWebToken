﻿global using AuthenticationJWT.Api.DataAccess;
global using AuthenticationJWT.Api.DataAccess.Repository;
global using AuthenticationJWT.Api.Extensions;
global using AuthenticationJWT.Api.Extensions.DB;
global using AuthentificationJWT.Api.Extensions;
global using AuthentificationJWT.Api.Extensions.Auth;
global using AuthentificationJWT.Api.Extensions.AWS;
global using AuthentificationJWT.Api.Extensions.SpoonacularFood;
global using AuthentificationJWT.Api.Helpers;
global using AuthentificationJWT.Api.Helpers.EndpointsDefinition;
global using AuthentificationJWT.Api.Middlewares;
global using AuthentificationJWT.Api.Models.Auth;
global using AuthentificationJWT.Api.Models.Config;
global using AuthentificationJWT.Api.Models.ProductModel;
global using AuthentificationJWT.Api.Models.Request;
global using AuthentificationJWT.Api.Models.Response;
global using AuthentificationJWT.Api.Models.UserModels;
global using AuthentificationJWT.Api.Services.Authentification;
global using AuthentificationJWT.Api.Services.SpoonacularFood;
global using AuthentificationJWT.Api.Services.SpoonacularFood.CircuitBreakerSpoonacularFood;
global using AuthentificationJWT.Api.Services.Token;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;
global using Newtonsoft.Json;
global using Polly;
global using Polly.Contrib.WaitAndRetry;
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.IdentityModel.Tokens.Jwt;
global using System.Net;
global using System.Security.Claims;
global using System.Text;
