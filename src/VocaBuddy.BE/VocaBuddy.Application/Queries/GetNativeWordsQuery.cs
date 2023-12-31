﻿using MediatR;
using VocaBuddy.Shared.Dtos;
using VocaBuddy.Shared.Models;

namespace VocaBuddy.Application.Queries;

public record GetNativeWordsQuery() : IRequest<Result<List<NativeWordDto>>>;
