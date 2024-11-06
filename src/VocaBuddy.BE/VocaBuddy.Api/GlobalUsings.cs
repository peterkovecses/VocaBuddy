﻿global using VocaBuddy.Shared.Dtos;
global using VocaBuddy.Shared.Models;
global using VocaBuddy.Shared.Errors;
global using VocaBuddy.Shared.Constants;
global using VocaBuddy.Api;
global using VocaBuddy.Api.Extensions;
global using VocaBuddy.Api.Middlewares;
global using VocaBuddy.Api.Services;
global using VocaBuddy.Application;
global using VocaBuddy.Application.Commands;
global using VocaBuddy.Application.Queries;
global using VocaBuddy.Application.Errors;
global using VocaBuddy.Application.Interfaces;
global using VocaBuddy.Infrastructure;
global using System.Net;
global using System.Text.Json;
global using System.Security.Claims;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.OpenApi.Models;
global using Microsoft.Data.SqlClient;
global using Microsoft.EntityFrameworkCore;
global using Serilog;
global using MediatR;