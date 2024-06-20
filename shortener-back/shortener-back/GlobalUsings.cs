﻿global using System.ComponentModel.DataAnnotations;
global using System.Data.Common;
global using System.IdentityModel.Tokens.Jwt;
global using System.Linq.Expressions;
global using System.Net;
global using System.Security;
global using System.Security.Claims;
global using System.Security.Cryptography;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using AutoMapper;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Diagnostics;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.WebUtilities;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.IdentityModel.Tokens;
global using shortener_back.Configurations;
global using shortener_back.DataAccess;
global using shortener_back.Dto;
global using shortener_back.Entities;
global using shortener_back.Repository;
global using shortener_back.Services;
global using shortener_back.Services.Security;
global using shortener_back.Services.Shorten;
global using shortener_back.Services.Users;
